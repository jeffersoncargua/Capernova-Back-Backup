using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using User.Managment.Data.Models;

namespace User.Managment.Data.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base (options)
        {

        }

        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SeedRoles(builder);

        }

        private static void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData
                (
                    new IdentityRole() { Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "ADMIN"},
                    new IdentityRole() { Name = "User", ConcurrencyStamp = "2", NormalizedName = "USER" },
                    new IdentityRole() { Name = "Student", ConcurrencyStamp = "3", NormalizedName = "STUDENT" }
                );
        }


    }
}
