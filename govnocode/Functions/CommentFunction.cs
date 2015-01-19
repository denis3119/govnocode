using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Antlr.Runtime.Tree;
using govnocode.Models;

namespace govnocode.Controllers
{
    public class CommentFunction
    {
        readonly UsersFunction _usersFunction = new UsersFunction();
        public void Delete(Comment comment)
        {        ApplicationDbContext _db = new ApplicationDbContext();
            _db.Entry(comment).State = EntityState.Deleted;
            _db.SaveChanges();
            _db.Dispose();
        }

        public void DeleteByUser(ApplicationUser user)
        {
            ApplicationDbContext _db = new ApplicationDbContext();
            foreach (var variable in _db.Comments.ToList().Where(x=>x.IdUser==user.Id))
            {
                Delete(variable);
            }
            _db.Dispose();
        }

        public void DeleteByPublication(Publication publication)
        {
            ApplicationDbContext _db = new ApplicationDbContext();
            foreach (var variable in _db.Comments.ToList().Where(x => x.IdPublication == publication.Id))
            {
                Delete(variable);
            }
            _db.SaveChanges();
            _db.Dispose();
        }

        public void Update(Comment comment)
        {
            ApplicationDbContext _db = new ApplicationDbContext();
            _db.Entry(comment).State = EntityState.Modified;
            _db.SaveChanges();
            _db.Dispose();
        }
        public Comment GetById(int id)
        {
            ApplicationDbContext _db = new ApplicationDbContext();
            return _db.Comments.ToList().FirstOrDefault(x => x.Id == id);
        }

        public List<Comment> GetByPublicationId(string id)
        {
            return GetByPublicationId(Int32.Parse(id));
        }
        public List<Comment> GetByPublicationId(int id)
        {
            ApplicationDbContext _db = new ApplicationDbContext();
            return _db.Comments.Where(x => x.IdPublication == id).ToList();
        }

        public List<RateComments> RateComments()
        {
            ApplicationDbContext _db = new ApplicationDbContext();
            return _db.RateComments.ToList();
        }

        public List<Comment> SetAllCommentsVoted(IEnumerable<Comment> list)
        {
            ApplicationDbContext _db = new ApplicationDbContext();
            var listcomments = new List<Comment>();
            foreach (var variableComment in list)
            {
                variableComment.Voted = true;
                listcomments.Add(variableComment);
            }
            _db.Dispose();
            return listcomments;
        }

        public List<Comment> SetCommentsVoted(IEnumerable<Comment> list, string userid)
        {

            var listcomments = new List<Comment>();
            foreach (var comment in list)
            {
                var firstOrDefault = _usersFunction.FindById(comment.IdUser);
                if (firstOrDefault == null) continue;
                comment.UserName = firstOrDefault.Name;
                var comment1 = comment;
                foreach (var ratecomm in RateComments().Where(ratecomm => comment1.Id == ratecomm.IdComment).Where(ratecomm => ratecomm.IdUser == userid))
                {
                    comment.Voted = true;
                }

                listcomments.Add(comment);
            }
            return listcomments;
        }

        public Int32 Rate(int idComment)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return db.RateComments.Where(x => x.IdComment == idComment).Sum(x => x.Rate);
        }

        public int ChangeRate(int idcomment, int rate,string userid)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            //var userid = User.Identity.GetUserId();
            var newComment = new RateComments()
            {
                IdUser = userid,
                IdComment = idcomment,
                Rate = rate
            };
            db.RateComments.Add(newComment);
            db.SaveChanges();
            var summ = Rate(idcomment);
            var comment = db.Comments.FirstOrDefault(x => x.Id == idcomment);
            if (comment != null)
            {
                comment.Rate = summ;
                db.Entry(comment).State = EntityState.Modified;
            }
            db.SaveChanges();
            db.Dispose();
            return summ;
        }

        public void AddComment(Comment comment)
        {
            
            using (var db = new ApplicationDbContext())
            {
                db.Comments.Add(comment);
                db.SaveChanges();
            }

        }

        public List<Comment>Top5()
        {    var listTop5Comments = new List<Comment>();
            using (var db = new ApplicationDbContext())
            {
                var comments = db.Comments.ToList();
                
                var topComments = comments.OrderByDescending(x => x.Rate);
               
               int[]  i = {0};
                foreach (var variable in topComments.TakeWhile(variable => i[0] <= 4))
                {
                    listTop5Comments.Add(variable);
                    i[0]++;
                }
            }
            return listTop5Comments;
        }
      

        public List<Comment> ToList()
        {
            using (var db = new ApplicationDbContext())
            {
                return db.Comments.ToList();
            }
        }
    }
}