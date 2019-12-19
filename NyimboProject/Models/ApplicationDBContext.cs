using Microsoft.AspNet.Identity.EntityFramework;
using NyimboProject.Models.Authentication;
using System.Data.Entity;

namespace NyimboProject.Models
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext() : base("DbContext")
        {
        }

        public DbSet<Song> Songs { get; set; }

        public static ApplicationDBContext Create()
        {
            return new ApplicationDBContext();
        }
    }
}