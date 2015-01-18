using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using govnocode.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace govnocode.Controllers
{
    [Culture]
    public class CommentController : Controller
    {
        readonly CommentFunction    _commentFunction = new CommentFunction();
        private ApplicationUserManager _userManager;

        private ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                _userManager = value;
            }
        }
      //  readonly ApplicationDbContext _dbContext = new ApplicationDbContext();

        [HttpPost]
        public bool Delete(int id)
        {

            var comment = _commentFunction.GetById(id);
            if (comment == null) return false;
            if (!User.IsInRole("admin") && User.Identity.GetUserId() != comment.IdUser) return false;
            //_dbContext.Entry(comment).State = EntityState.Deleted;
            //_dbContext.SaveChanges();
             _commentFunction.Delete(comment);
            return true;
        }
        [HttpPost]
        public bool Update(string text, int id)
        {
            
            var comment = _commentFunction.GetById(id);
            
            if (comment == null) return false;
            if (!User.IsInRole("admin") && User.Identity.GetUserId() != comment.IdUser) return false;
            comment.Text = text;
            _commentFunction.Update(comment);
            return true;
        }

        [HttpPost]
        public JsonResult RequestEdit(int idcomment)
        {
            var iduser = User.Identity.GetUserId();
            var comment = _commentFunction.GetById(idcomment);
            if ((User.IsInRole("admin") || comment.IdUser == iduser))
            {
                return new JsonResult(){Data = true};
            }
            return new JsonResult(){Data = false};
        }
        [HttpPost]
        public JsonResult All(string id)
        {
            var userid = User.Identity.GetUserId();
            var list = _commentFunction.GetByPublicationId(id);
            if (!User.Identity.IsAuthenticated)
            {
 
               return new JsonResult()
                {
                    Data = _commentFunction.SetAllCommentsVoted(list)
                };
            }
            var listcomments = _commentFunction.SetCommentsVoted(list,userid);
            return new JsonResult { Data = listcomments };

        }

        [HttpPost]
        public int Rateint(int idcomment)
        {
            return _commentFunction.Rate(idcomment);
        }
        [HttpPost]
        public JsonResult Rate(int idcomment)
        {
            return new JsonResult() { Data = Rateint(idcomment) };
        }
        [HttpPost]
        public JsonResult Up(int idcomment)
        {
            return JsonResult(idcomment, 1);
        }
        [HttpPost]
        public JsonResult Down(int idcomment)
        {
            return JsonResult(idcomment, -1);
        }
        private JsonResult JsonResult(int idcomment, int rate)
        {
            var userid = User.Identity.GetUserId();
            return new JsonResult() { Data = _commentFunction.ChangeRate(idcomment,rate,userid) };
        }

        [HttpPost]
        public void Add(string text, string id)
        {
            if (!User.Identity.IsAuthenticated) return;
            var usid = User.Identity.GetUserId();
            var comment = new Comment()
            {
                IdPublication = Int32.Parse(id),
                IdUser = usid,
                Text = text.Trim(),
                UserName = UserManager.FindById(usid).Name,
                Date = DateTime.Now
            };
            _commentFunction.AddComment(comment);
        }
    }
}