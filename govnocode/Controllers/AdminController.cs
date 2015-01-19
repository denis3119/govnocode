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
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {

        private ApplicationUserManager _userManager;
        private UsersFunction _usersFunction=new UsersFunction();

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

        readonly ApplicationDbContext _t = new ApplicationDbContext();
        public ActionResult Edit(string id)
        {
            return View(_t.Users.Find(id));
        }
        //[Authorize(Roles = "admin,user")]
        [HttpPost]
        public async Task<ViewResult> Edit(ApplicationUser user)
        {
            var error = _usersFunction.UsedName(user.Name);
            if (error)
            {
                ModelState.AddModelError("", string.Format(Resources.Resource.name_already+"({0})",user.Name));
                return View(UserManager.FindById(user.Id));
            }
            _usersFunction.Update(user);
            return View("Edit",UserManager.FindById(user.Id));
        }
        public ActionResult Index()
        {
           
            return View(_t.Users);
        }

        public ActionResult Details(string id)
        {
            return View(_t.Users.Find(id));
        }

        public ActionResult Delete(string id)
        {
            return View((_t.Users.Find(id)));
        }
        [HttpPost]
        public ActionResult Delete(ApplicationUser user, FormCollection collection)
        {
            //var usersearch = UserManager.FindById(user.Id);
            //UserManager.Delete(usersearch);
            _usersFunction.Delete(user);
            return View("Index",UserManager.Users);
        }
    }
}