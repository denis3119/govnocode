using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using govnocode.Models;

namespace govnocode.Controllers
{
    public class PublicationsFunction
    {
      //  readonly 
        public List<Publication> Top5()
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();
            var listPublication = dbContext.Publications.ToList();
            var topPublication = listPublication.OrderByDescending(x => x.Rate);


            var toplist = new List<Publication>();
            int[] i = {0};
            foreach (var variable in topPublication.TakeWhile(variable => i[0] <= 4))
            {
                toplist.Add(variable);
                i[0]++;
            }
            return toplist;
        }
        public void Delete(Publication publication)
        {
            CommentFunction commentFunction = new CommentFunction();
            ApplicationDbContext db = new ApplicationDbContext();
            commentFunction.DeleteByPublication(publication);
            DeleteTags(publication);
            db.Entry(publication).State = EntityState.Deleted;
            db.SaveChanges();
             db.Dispose();
            
        }

        public void DeleteTags(Publication publication)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            foreach (var VARIABLE in db.Tags.ToList().Where(x=>x.PublicationId==publication.Id))
            {
                db.Entry(VARIABLE).State=EntityState.Deleted;

            }
            db.SaveChanges();
            db.Dispose();

        }
        public void PublicationDeleteByUser(ApplicationUser user)
        {
            ApplicationDbContext _db = new ApplicationDbContext();
            foreach (var variable in _db.Publications.ToList().Where(x => x.UserId == user.Id))
            {
                Delete(variable);
            }
            _db.Dispose();
        }

        public List<Publication> Top5ByLanguage(string lang)
        {
            var top5 = new List<Publication>();
            using (var db = new ApplicationDbContext())
            {
                var listPublication = db.Publications.ToList();
                var topPublication = listPublication.OrderByDescending(x => x.Rate);
                var topCSharp = topPublication.Where(x => x.Lang == lang);

                int[]  i = {0};
                
                foreach (var variable in topCSharp.TakeWhile(variable => i[0] <= 4))
                {
                    top5.Add(variable);
                    i[0]++;
                }
            }
            return top5;
        }

        public List<Publication> Pages(int? page)
        {
            var listResult = new List<Publication>();
            using (var dbContext = new ApplicationDbContext())
            {
                var arrayPublication = dbContext.Publications.ToArray().OrderByDescending(x => x.Date).ToArray();
                if (page <= 1 || page == null) page = 1;
                const int countNewsOnPage = 6;
                var from = (page - 1)*countNewsOnPage;
                var to = page*countNewsOnPage;
                if (to >= arrayPublication.Length) to = arrayPublication.Length;
                for (var j = (int) @from; j < to; j++)
                {

                    listResult.Add(arrayPublication[j]);
                }
            }
            return listResult;
        }

        public bool PosterUser(Int32 publicationId,ApplicationUser user)
        {
            using (var db= new ApplicationDbContext())
            {
                if (db.Publications.Where(x => x.Id == publicationId).ToList().Any(x => x.UserId == user.Id))
                {
                    return true;
                }
            }
           return false;
        }

        public int RatePublications(ApplicationUser user)
        {
            var point = 0;
            using (var db = new ApplicationDbContext())
            {
                //if (db.Publications.Where(x => x.Id == publicationId).ToList().Any(x => x.UserId == user.Id))
                //{
                //    return true;
                //}
                point += db.Publications.ToList().Where(x => x.UserId == user.Id).Sum(variable1 => db.RatePublications.ToList().Where(x => x.IdPublication == variable1.Id).Sum(variable => variable.Rate));
            }
            return point;
        }
    }
}