using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using Undye.Web.Models;

namespace Undye.Web.Controllers
{
    [RequireHttps]
    public class PetitionController : Controller
    {
        private ApplicationUserManager _userManager;

        public PetitionController()
        {

        }

        public PetitionController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index()
        {
            var users = UserManager.Users.Where(u => u.Signed);
            var model = new IndexPetitionViewModel { Petitions = users.Count() };

            return View(model);
        }

        public PartialViewResult Sign()
        {
            var model = new SignPetitionViewModel();

            if (User.Identity.IsAuthenticated)
            {
                var user = UserManager.FindById(User.Identity.GetUserId());
                model.Signed = user.Signed;
            }
            else
            {
                model.Signed = false;
            }

            return PartialView(model);
        }

        public ActionResult SignMobile()
        {
            return View();
        }

        public ActionResult Facts()
        {
            return View();
        }

        public ActionResult Faq()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Sign(SignPetitionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            user.Signed = model.Signed;

            var result = await UserManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                AddErrors(result);
            }
            return RedirectToAction("Index");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

    }
}