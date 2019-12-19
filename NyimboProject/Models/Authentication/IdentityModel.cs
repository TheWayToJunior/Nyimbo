using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NyimboProject.Models.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        public string NickName { get; set; }

        public virtual ICollection<Song> Songs { get; set; }
    }

    public class ApplicationUserMenager : UserManager<ApplicationUser>
    {
        public ApplicationUserMenager(IUserStore<ApplicationUser> store) : base(store)
        {
        }

        public static ApplicationUserMenager Create(IdentityFactoryOptions<ApplicationUserMenager> options, 
            IOwinContext context)
        {
            ApplicationDBContext db = context.Get<ApplicationDBContext>();

            return new ApplicationUserMenager(new UserStore<ApplicationUser>(db));
        }
    }
}