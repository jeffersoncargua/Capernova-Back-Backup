// <copyright file="ApplicationUser.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Identity;

namespace User.Managment.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }

        public string? LastName { get; set; }

        public string? Ciudad { get; set; }

        // public string? DirecionPrincipal { get; set; }

        // public string? DireccionSecundaria { get; set; }
    }
}