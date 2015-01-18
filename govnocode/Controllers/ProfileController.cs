using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using govnocode.Models;
using Microsoft.AspNet.Identity;

namespace govnocode.Controllers
{
    [Culture]
    public class ProfileController : Controller
    {
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
        // GET: Profile
        public ActionResult Details(string id="")
        {
            var dbContext = new ApplicationDbContext();
            if (id == "") return RedirectToAction("Index", "Home");
            var publicationList=dbContext.Publications.Where(x => x.UserId == id).ToList();
            ViewData["Pub"] = publicationList.OrderByDescending(x=>x.Rate).ToList();
            ViewData["Comments"] = dbContext.Comments.ToList();
            var user = dbContext.Users.First(x => x.Id == id);
            user.Rating = GetUserRate(user);
            dbContext.Dispose();
            return View(user);
        }
        [HttpPost]
        public bool SetUserName(string name)
        {
           // var dbContext = new ApplicationDbContext();
            var users = new UsersFunction();
            if (!User.Identity.IsAuthenticated) return false;
            var userid = User.Identity.GetUserId();
            var user = users.FindById(userid);
            if (users.UsedName(name))
            {    
                return false;
            }
            if (user == null) return false;
            user.Name = name;
            users.Update(user);
            return true;
        }

        private static bool CheckUrlValid(string source)
        {
            Uri uriResult;
            return Uri.TryCreate(source, UriKind.Absolute, out uriResult) && uriResult.Scheme == Uri.UriSchemeHttp;
        }
        [HttpPost]
        public void SetLinkImage(string image)
        {
            if (string.IsNullOrEmpty(image)) return;
            var userFunction = new UsersFunction();
            if (!User.Identity.IsAuthenticated) return;
            var user = userFunction.GetById(User.Identity.GetUserId());
            user.LinkAvatar = image;
            userFunction.Update(user);
        }
    }
}