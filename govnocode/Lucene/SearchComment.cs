using System.Collections.Generic;
using System.IO;
using System.Linq;
using govnocode.Models;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;

namespace govnocode.Lucene
{
    public class SearchComment
    {
        private const string Path = @"C:\Test";

        // Названия полей, необходимых для индексирования.
        private const string IdName = "Id";
        private const string TextName = "Text";
        // Максимальное количество найденных записей.
        private const int Size = 10;

        // Версия поискового движка.
        private readonly Version _version = Version.LUCENE_30;

        // Анализатор поискового индекса.
        private readonly Analyzer _analyzer = new StandardAnalyzer(Version.LUCENE_30);
        private readonly ApplicationDbContext _context = new ApplicationDbContext();
        private void IndexCreate()
        {
            var indexDirectory = new DirectoryInfo(Path);

            // 1.
            if (indexDirectory.Exists)
                indexDirectory.Delete(true);

            FSDirectory entityDirectory = null;
            IndexWriter writer = null;

            try
            {
                // 2.
                entityDirectory = FSDirectory.Open(indexDirectory);
                writer = new IndexWriter(
                    entityDirectory,
                    _analyzer,
                    true,
                    IndexWriter.MaxFieldLength.UNLIMITED);

                // 3.

                {
                    foreach (var publication in _context.Comments.ToList())
                    {
                        var document = new Document();

                        document.Add(
                            new Field(
                                IdName,
                                publication.Id.ToString(),
                                Field.Store.YES,
                                Field.Index.NOT_ANALYZED,
                                Field.TermVector.NO));

                       
                        document.Add(
                            new Field(
                                TextName,
                                publication.Text,
                                Field.Store.YES,
                                Field.Index.ANALYZED,
                                Field.TermVector.WITH_POSITIONS_OFFSETS));
                       
                        writer.AddDocument(document);
                    }
                }

                // 4.
                writer.Optimize(true);
            }
            finally
            {
                if (writer != null)
                    writer.Close();

                if (entityDirectory != null)
                    entityDirectory.Close();
            }
        }

        public SearchComment()
        {
            IndexCreate();
        }
        public IEnumerable<Comment> Find(string query)
        {
            // 1.
            if (string.IsNullOrWhiteSpace(query))
                return Enumerable.Empty<Comment>();

            var indexDirectory = new DirectoryInfo(Path);

            FSDirectory entityDirectory = null;
            IndexSearcher searcher = null;

            try
            {
                // 2.
                entityDirectory = FSDirectory.Open(indexDirectory);
                searcher = new IndexSearcher(entityDirectory, true);

                // 3.
                var documentBooks = FindDocumentComment(query, searcher);

                // 4.

                return documentBooks.Select(db => db.Value).ToList();
            }
            finally
            {
                if (searcher != null)
                    searcher.Close();

                if (entityDirectory != null)
                    entityDirectory.Close();
            }
        }
        private IEnumerable<KeyValuePair<int, Comment>> FindDocumentComment(string query, Searcher searcher)
        {
            // 1.
            var parser = new MultiFieldQueryParser(
                _version,
                new[] {TextName},
                _analyzer);

            // 2.
            var scoreDocs = searcher.Search(parser.Parse(query), Size).ScoreDocs;

            // 3.
            var documentBookIds = scoreDocs.Select(x => new KeyValuePair<int, int>
                (
                x.Doc,
                int.Parse(searcher.Doc(x.Doc).Get(IdName))
                )).ToList();

            // 4.
            var bookIds = documentBookIds.Select(db => db.Value);

            return _context.Comments.ToList()
                .Where(b => bookIds.Contains(b.Id))
                .ToList()
                .Select(b =>
                    new KeyValuePair<int, Comment>(
                        documentBookIds.Single(db => db.Value == b.Id).Key, b));

        }
    }
}