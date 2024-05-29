using Microsoft.AspNetCore.Identity;

namespace User.Managment.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }

        public string? LastName { get; set; }

        public string? Cuidad { get; set; }
    }
}
