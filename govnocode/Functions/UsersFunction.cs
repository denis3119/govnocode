using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Linq;
using govnocode.Models;
using Lucene.Net.Search;
using Microsoft.AspNet.Identity;

namespace govnocode.Controllers
{
    public class UsersFunction
    {
        //readonly ApplicationDbContext _db = new ApplicationDbContext();
        //readonly PublicationsFunction _publicationsFunction = new PublicationsFunction();
        //readonly CommentFunction _commentFunction = new CommentFunction();
        private double GetUserRate(ApplicationUser user)
        {
            var dbContext = new ApplicationDbContext();
            var summPublicationRate = dbContext.RatePublications.Where(x => x.IdUser == user.Id).ToList().Sum(x => x.Rate);
            var summCommentRate = dbContext.RateComments.Where(x => x.IdUser == user.Id).ToList().Sum(x => x.Rate);
            var ratePublication = (double)summPublicationRate / 50;
            var rateComment = (double)summCommentRate / 100;
            dbContext.Dispose();
            return rateComment + ratePublication;
        }
        public List<ApplicationUser> Top5()
        {
            var dbContext = new ApplicationDbContext();
            var listUsersRate = dbContext.Users.ToList();
            var sortListUser = new List<ApplicationUser>();
            foreach (var variable in listUsersRate)
            {
                variable.Rating = GetUserRate(variable);
                sortListUser.Add(variable);
            }
            dbContext.Dispose();
            return sortListUser;
        }
        public  void Delete(ApplicationUser user)
        {
            ApplicationDbContext _db = new ApplicationDbContext();
        PublicationsFunction _publicationsFunction = new PublicationsFunction();
           
            CommentFunction _commentFunction = new CommentFunction();
             _commentFunction.DeleteByUser(user);
            _publicationsFunction.PublicationDeleteByUser(user);
            _db.Entry(user).State = EntityState.Deleted;
            _db.SaveChanges();
            _db.Dispose();
            
        }

         public  List<ApplicationUser> ToList()
        {
            ApplicationDbContext _db = new ApplicationDbContext();
             return _db.Users.ToList();
         }

        public bool UsedName(string name)
         {
             ApplicationDbContext db = new ApplicationDbContext();
            var result = db.Users.ToList().Any(variable => variable.Name == name);
            db.Dispose();
            return result;
        }

        public void Update(ApplicationUser user)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            db.Users.AddOrUpdate(user);
            db.SaveChanges();
            //db.Entry(user).State=EntityState.Modified;
            //db.SaveChanges();
            db.Dispose();
        }

        public ApplicationUser FindById(String id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return db.Users.FirstOrDefault(x => x.Id == id);
        }

        public ApplicationUser GetById(string id)
        {
            return FindById(id);
        }
    }
}