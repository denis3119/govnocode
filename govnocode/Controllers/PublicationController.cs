using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using govnocode.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace govnocode.Controllers
{

    [Culture]
    public class PublicationController : Controller
    {

        private ApplicationUserManager _userManager;

        private ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }
        readonly ApplicationDbContext _dbContext = new ApplicationDbContext();
        public ActionResult Edit(int id)
        {
            var tags = _dbContext.Tags.Where(x => x.PublicationId == id).ToList();
            var publication = _dbContext.Publications.FirstOrDefault(x => x.Id == id);
            if (publication != null)
            {
                publication.TagString = "";
                foreach (var variable in tags)
                {
                    publication.TagString += variable.Text+",";
                }
                return View(publication);
            }
            return View(publication);
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ViewResult> Edit(Publication publication)
        {
            var tagsPublications = _dbContext.Tags.Where(x => x.PublicationId == publication.Id).ToList();
            var firstOrDefault = _dbContext.Publications.FirstOrDefault(x => x.Id == publication.Id);
            if (firstOrDefault != null)
            {
                var dataOld = firstOrDefault.Date;
                var nowtags = publication.TagString.Split(',').Where(x=>!string.IsNullOrEmpty(x));
                foreach (var variable in tagsPublications.Where(variable => !nowtags.Contains(variable.Text)))
                {
                    _dbContext.Entry(variable).State=EntityState.Deleted;
                }
                publication.Date = dataOld;
            }
            _dbContext.Publications.AddOrUpdate(publication);

            await _dbContext.SaveChangesAsync();

             ModelState.AddModelError("",Resources.Resource.save_changes);
            return View("Edit", publication);
        }
        public ActionResult Details(int id=-1)
        {
           
            var publication = _dbContext.Publications.FirstOrDefault(x => x.Id == id);
            if (id == -1) return RedirectToAction("Index","Home");
            if (publication == null) return View(new Publication());
            publication.UserId = UserManager.FindById(publication.UserId).Id;
                var t = _dbContext.Publications.FirstOrDefault(x => x.Id == id);
            ViewData["comments"] = _dbContext.Comments.Where(x=>x.IdPublication==id).ToList();
            ViewData["username"] = UserManager.FindById(publication.UserId).Name;
            if (!User.Identity.IsAuthenticated) 
                ViewData["Voted"] = true; 
            else 
                ViewData["Voted"] = Voted(User.Identity.GetUserId(),publication.Id);
            
            return View(publication);
        }

        private bool Voted(String idUser,int idPublication)
        {
            var pubRates = _dbContext.RatePublications.Where(x => x.IdPublication == idPublication).ToList();
            var res = pubRates.Count(x => x.IdUser==idUser);
            return res == 1;
        }
        [Authorize]
        public ActionResult Delete(int id)
        {
            var userid = User.Identity.GetUserId();
            var publication = _dbContext.Publications.FirstOrDefault(x => x.Id == id);
            if (publication != null && userid == publication.UserId) return View(publication);
            ModelState.AddModelError("123", "Не ваши новости");
            return View("Index", _dbContext.Publications.Where(x => x.UserId == userid));
        }
        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Delete(Publication publication)
        {
            var userid = User.Identity.GetUserId();
            publication = _dbContext.Publications.First(x => x.Id == publication.Id);
            if (userid != publication.UserId&&!User.IsInRole("admin"))
            {
                ModelState.AddModelError("", "error");
                return View("Index", _dbContext.Publications);
            }
            _dbContext.Entry(publication).State = EntityState.Deleted;
            var tagsPublication = _dbContext.Tags.Where(x => x.PublicationId == publication.Id);
            foreach (var variable in tagsPublication)
            {
                _dbContext.Entry(variable).State = EntityState.Deleted;
            }
            var commPublication = _dbContext.Comments.Where(x => x.IdPublication == publication.Id);
            foreach (var variable in commPublication)
            {
                _dbContext.Entry(variable).State = EntityState.Deleted;
            }
            _dbContext.SaveChanges();

            return View("Index", _dbContext.Publications.Where(x => x.UserId == userid));
        }
        [Authorize(Roles = "admin,user")]
        [ValidateInput(false)]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "admin,user")]
        [ValidateInput(false)]
        public ActionResult Create(Publication publication)
        {
            if (string.IsNullOrEmpty(publication.Text) || string.IsNullOrEmpty(publication.Title) ||
                string.IsNullOrEmpty(publication.Code) || string.IsNullOrEmpty(publication.Lang)||string.IsNullOrEmpty(publication.TagString))
                return View(publication);
            publication.Text = publication.Text.Trim();
            publication.UserId = User.Identity.GetUserId();
            var tags = publication.TagString.Split(',').Where(x => !string.IsNullOrEmpty(x)).ToList();
            var tagsnew = tags.Select(variable => new Tag() { Text =variable }).ToList();
            foreach (var variable in tagsnew)
            {
                
                publication.Tags.Add(variable);
            }
            publication.Date = DateTime.Now;
            _dbContext.Entry(publication).State = EntityState.Added;
          
            //     _dbContext.Publications.Add(publication);
            //   _dbContext.Publications.Add(publication);
            _dbContext.SaveChanges();
          //  ModelState.;
              
            return View("Index",_dbContext.Publications.Where(x=>x.UserId==publication.UserId).ToList());
        }
        [Authorize]
        public ActionResult Index()
        {
            var userid = User.Identity.GetUserId();
            var mypublication = _dbContext.Publications.Where(publication => publication.UserId == userid).ToList();
            return View(mypublication);
        }

        [HttpPost]
        public bool UpRate(int rate, int idPublication)
        {
            if (rate < 1 && rate > 5) return false;
            var iduser = User.Identity.GetUserId();
            if (!User.Identity.IsAuthenticated) return false;
            if (_dbContext.RatePublications.Where(x => x.IdUser == iduser).Count(x => x.IdPublication == idPublication) !=0) return false;
            _dbContext.RatePublications.Add(new RatePublication()
            {
               IdUser =  iduser,
               IdPublication = idPublication,
               Rate = (rate)
            });
            _dbContext.SaveChanges();
            var publication = _dbContext.Publications.FirstOrDefault(x => x.Id == idPublication);
            var rankNumber =
                _dbContext.RatePublications.Where(x => x.IdPublication == idPublication).ToList().Average(x => x.Rate);
            if (publication != null) publication.Rate = rankNumber;
              _dbContext.Entry(publication).State=EntityState.Modified;
            _dbContext.SaveChanges();
            
            return true;
        }
        [HttpPost]
        public int GetRate(int idPublication)
        {
            var publicationRates= _dbContext.RatePublications.Where(x => x.IdPublication == idPublication).ToList();
            if (publicationRates.Count == 0) return 0;
            var average = publicationRates.Average(x => x.Rate);
            var res = Convert.ToInt32(Math.Round(average));
            return res;
        }
        

    }
}