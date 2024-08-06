using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using User.Managment.Data.Models;
using User.Managment.Data.Models.Course;
using User.Managment.Data.Models.Managment;
using User.Managment.Data.Models.Student;
using Course = User.Managment.Data.Models.Course.Course;

namespace User.Managment.Data.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base (options)
        {

        }

        public DbSet<Publicidad> PublicidadTbl { get; set; }
        //public DbSet<Course> CourseTbl { get; set; }
        public DbSet<Teacher> TeacherTbl { get; set; }
        public DbSet<Student> StudentTbl { get; set; }
        public DbSet<Course> CourseTbl { get; set; }
        public DbSet<Capitulo> CapituloTbl { get; set; }
        public DbSet<Video> VideoTbl { get; set; }
        public DbSet<Deber> DeberTbl { get; set; }
        public DbSet<Prueba> PruebaTbl { get; set; }
        public DbSet<NotaDeber> NotaDeberTbl { get; set; }
        public DbSet<NotaPrueba> NotaPruebaTbl { get; set; }
        public DbSet<EstudianteVideo> EstudianteVideoTbl { get; set; }






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
                    new IdentityRole() { Name = "Student", ConcurrencyStamp = "3", NormalizedName = "STUDENT" },
                    new IdentityRole() { Name = "Teacher", ConcurrencyStamp = "4", NormalizedName = "TEACHER" },
                    new IdentityRole() { Name = "Secretary", ConcurrencyStamp = "5", NormalizedName = "SECRETARY" }
                );
        }


    }
}
