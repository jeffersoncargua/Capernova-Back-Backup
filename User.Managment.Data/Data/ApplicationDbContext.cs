using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using User.Managment.Data.Models;
using User.Managment.Data.Models.Managment;

namespace User.Managment.Data.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base (options)
        {

        }

        public DbSet<Publicidad> PublicidadTbl { get; set; }
        public DbSet<Course> CourseTbl { get; set; }



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
