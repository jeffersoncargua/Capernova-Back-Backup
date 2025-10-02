// <copyright file="LoginDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace User.Managment.Data.Models.Authentication.Login
{
    public class LoginDto
    {
        [Required(ErrorMessage = "El correo de usuario es requerido")]
        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "La constraseña es requerida")]
        public string? Password { get; set; }
    }
}