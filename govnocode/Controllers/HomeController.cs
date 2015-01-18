using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using govnocode.Models;

namespace govnocode.Controllers
{     
    [Culture]
    public class HomeController : Controller
    {
       // readonly ApplicationDbContext _dbContext = new ApplicationDbContext();
        
        public ActionResult Index(int? page)
        {
            UsersFunction usersFunction = new UsersFunction();
            PublicationsFunction publicationsFunction = new PublicationsFunction();
            CommentFunction commentFunction = new CommentFunction();
            #region top
            /*top5user*/
           
            ViewData["Top5Users"] = usersFunction.Top5();
            /*top5user*/
            //top5Posts
           
            ViewData["Top5Posts"] = publicationsFunction.Top5();
            //top5Posts
            //top5Comments
            ViewData["Comments"] = commentFunction.ToList();
            ViewData["Top5Comments"] = commentFunction.Top5();
            //top5Comments
            //top5C#
           

            ViewData["Top5C#"] = publicationsFunction.Top5ByLanguage("C#");
            //top5C#
            //top5Java
           
            ViewData["Top5Java"] = publicationsFunction.Top5ByLanguage("Java");
            //top5Java
            //top5Pyhton
            
            ViewData["Top5Python"] = publicationsFunction.Top5ByLanguage("Python");
            //top5Java
            //top5Pyhton
            
            ViewData["Top5Ruby"] = publicationsFunction.Top5ByLanguage("Ruby");
            //top5Java
            //allPublication
           
            //Publication
            #endregion
            #region pages

            if (page <= 1 || page == null) page = 1;
            ViewData["AllPublication"] =publicationsFunction.Pages(page);
            ViewData["Page"] = page;
            #endregion
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult ChangeCulture(string lang)
        {
            string returnUrl = Request.UrlReferrer.AbsolutePath;
            // Список культур
            List<string> cultures = new List<string>() { "ru", "en" };
            if (!cultures.Contains(lang))
            {
                lang = "ru";
            }
            // Сохраняем выбранную культуру в куки
            HttpCookie cookie = Request.Cookies["lang"];
            if (cookie != null)
                cookie.Value = lang;   // если куки уже установлено, то обновляем значение
            else
            {

                cookie = new HttpCookie("lang");
                cookie.HttpOnly = false;
                cookie.Value = lang;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            return Redirect(returnUrl);
        }
        public ActionResult ChangeThemes(string them)
        {
            var returnUrl = Request.UrlReferrer.AbsolutePath;
            // Список культур
            var themesList = new List<string>() { "css", "css2" };
            if (!themesList.Contains(them))
            {
                them = "css";
            }
            // Сохраняем выбранную культуру в куки
            var cookie = Request.Cookies["them"];
            if (cookie != null)
                cookie.Value = them;   // если куки уже установлено, то обновляем значение
            else
            {

                cookie = new HttpCookie("them")
                {
                    HttpOnly = false, Value = them, Expires = DateTime.Now.AddYears(1)
                };
            }
            Response.Cookies.Add(cookie);
            return Redirect(returnUrl);
        }

    }
}