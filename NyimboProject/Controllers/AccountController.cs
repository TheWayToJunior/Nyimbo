using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using NyimboProject.Models.Authentication;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NyimboProject.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationUserMenager _UserMenager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserMenager>();
            }
        }

        private IAuthenticationManager _AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(Login login, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await _UserMenager.FindAsync(login.UserName, login.Password);

                if (user != null)
                {
                    var claims = await _UserMenager
                        .CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

                    _AuthenticationManager.SignOut();
                    _AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claims);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Неверный пароль или имя пользователя");
            }

            return View(login);
        }

        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Registration(Registration registration, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    UserName = registration.Email,
                    Email = registration.Email,
                    NickName = registration.NickName
                };

                var result = await _UserMenager.CreateAsync(user, registration.Password);

                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");

                foreach (var item in result.Errors)
                    ModelState.AddModelError("", item);
            }

            return View(registration);
        }

        [Authorize]
        public ActionResult LoginOff()
        {
            _AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            Session.Abandon();
            Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Menu()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = _UserMenager.FindByEmail(User.Identity.Name);

                if (user != null)
                {
                    ViewData["NickName"] = user.NickName;
                    ViewData["Email"] = user.Email;

                    return PartialView("ProfileMenu");
                }
            }

            return PartialView("LogMenu");
        }
    }
}