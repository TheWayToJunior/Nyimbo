using Microsoft.AspNet.Identity.Owin;
using NyimboProject.Models.Authentication;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NyimboProject.Controllers
{
    [Authorize]
    public class UserContentController : Controller
    {
        private ApplicationUserMenager _UserMenager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserMenager>();
            }
        }

        public async Task<ActionResult> MusicStor()
        {
            var user = await _UserMenager.FindByEmailAsync(User.Identity.Name);
            ViewBag.Name = user.NickName;

            return View(user.Songs.ToList());
        }
    }
}