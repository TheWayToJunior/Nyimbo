using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using NyimboProject.Models;
using NyimboProject.Models.Authentication;
using Owin;

[assembly: OwinStartup(typeof(NyimboProject.App_Start.Startup))]

namespace NyimboProject.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Дополнительные сведения о настройке 
            //приложения см. на странице https://go.microsoft.com/fwlink/?LinkID=316888

            app.CreatePerOwinContext<ApplicationDBContext>(ApplicationDBContext.Create);
            app.CreatePerOwinContext<ApplicationUserMenager>(ApplicationUserMenager.Create);
            app.UseCookieAuthentication(new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions()
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
        }
    }
}
