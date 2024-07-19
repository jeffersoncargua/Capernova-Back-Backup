using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using User.Managment.Data.Models.Managment;

namespace User.Managment.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }

        public string? LastName { get; set; }

        public string? Ciudad { get; set; }


    }
}
