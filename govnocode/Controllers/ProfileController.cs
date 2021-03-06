﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using govnocode.Functions;
using govnocode.Models;
using Microsoft.AspNet.Identity;

namespace govnocode.Controllers
{
    [Culture]
    [Authorize]
    public class ProfileController : Controller
    {
        private double GetUserRate(ApplicationUser user)
        {
            var dbContext = new ApplicationDbContext();
            //var listRate = dbContext.RatePublications.Where(x => (new PublicationsFunction().PosterUser(x.IdPublication,user))).ToList();
            var listRate = new PublicationsFunction().RatePublications(user);
            var summPublicationRate = listRate;
            var summCommentRate = new CommentFunction().SummRateCommentsByUser(user);
            var ratePublication = (double)summPublicationRate / 50;
            var rateComment = (double)summCommentRate / 100;
            dbContext.Dispose();
            return rateComment + ratePublication;
        }
        // GET: Profile
        public ActionResult Details(string id="")
        {
            
            var dbContext = new ApplicationDbContext();
            if (id == "")
            {
                if (!User.Identity.IsAuthenticated)
                {
                    dbContext.Dispose();
                    return RedirectToAction("Index", "Home");
                }
                id= User.Identity.GetUserId();
            }
           var userId= User.Identity.GetUserId();
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
            if (name.Length > 15) return false;
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

        [HttpPost]
        public JsonResult GetLinkImage(string id)
        {
             return new JsonResult(){Data = new UsersFunction().GetById(id).LinkAvatar};
        }
    }
}