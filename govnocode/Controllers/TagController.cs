using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using govnocode.Models;

namespace govnocode.Controllers
{
    [Culture]
    public class TagController : Controller
    {
        private readonly ApplicationDbContext _dbContext = new ApplicationDbContext();
        // GET: Tag
        [HttpPost]
        public JsonResult All()
        {
            var model = _dbContext.Tags.Distinct().ToList();
            var notrepeat = new List<string>();
            foreach (var variable in model.Where(variable => !notrepeat.Contains(variable.Text)))
            {
                notrepeat.Add(variable.Text);
            }
           // var i = model.Select(variable => variable.Text).ToList();

            return new JsonResult(){Data = notrepeat};
        }
        public ActionResult Index(string tag)
        {
            if (tag != null)
            {
                ViewData["Pub"] = _dbContext.Publications.ToList().Where(x => x.TagString.Split(',').Contains(tag)).ToList();
                ViewData["Comments"] = _dbContext.Comments.ToList();
                return View("Tag");
            }
            var tags = _dbContext.Tags.Distinct().ToList();
            var notrepeat = new List<string>();
            foreach (var variable in tags.Where(variable => !notrepeat.Contains(variable.Text)))
            {
                notrepeat.Add(variable.Text);
            }
            ViewData["Tags"] = notrepeat;
            return View();
        }
        public ActionResult Tag(string tag)
        {
            if (tag == null) tag = "";
            ViewData["Pub"] = _dbContext.Publications.ToList().Where(x => x.TagString.Split(',').Contains(tag)).ToList();
            ViewData["Comments"] = _dbContext.Comments.ToList();
            return View();
        }
    }
}