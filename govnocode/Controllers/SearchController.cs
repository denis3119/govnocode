using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using govnocode.Lucene;
using govnocode.Models;
using Microsoft.AspNet.Identity;

namespace govnocode.Controllers
{
    [Culture]
    public class SearchController : Controller
    {      // Путь к каталогу, хранящему поисковый индекс.
        // GET: Search
        public ActionResult Index(string s)
        {    //IndexCreate();
            var findPublication = new SearchPublication();
            
            var publications = findPublication.Find(s);
            ViewData["Publications"] = publications;
            var findComent = new SearchComment();
            var t = findComent.Find(s);
            ViewData["Comments"] = findComent.Find(s);
            var db = new ApplicationDbContext();
            ViewData["Users"] = db.Users.ToList();
            var newPublication = db.Publications.ToList();
            foreach (var publ in newPublication.Where(publ => publ.Title.Length > 30))
            {
                publ.Title = publ.Title.Substring(0, 29) + "...";
            }
            ViewData["AllPublication"] = newPublication;
            db.Dispose();
            return View();
        }
    }

}