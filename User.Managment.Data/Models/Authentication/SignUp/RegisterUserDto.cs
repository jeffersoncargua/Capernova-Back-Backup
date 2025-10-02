// <copyright file="RegisterUserDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace User.Managment.Data.Models.Authentication.SignUp
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido ")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "El apellido de usuario es requerido ")]
        public string? LastName { get; set; }

        [EmailAddress(ErrorMessage = "No cumple con el formato de correo electrónico")]
        [Required(ErrorMessage = "El correo del usuario es requerido ")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "La contraseña de usuario es requerido ")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "La confirmación de contraseña de usuario es requerido ")]
        [Compare("Password")]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "El teléfono de usuario es requerido ")]
        [StringLength(10, ErrorMessage = "Deben ser 10 digitos")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "La cuidad de residencia de usuario es requerido")]
        public string? City { get; set; }

        // [Required(ErrorMessage = "La direccion principal de residencia de usuario es requerido")]
        // public string? DirectionMain { get; set; } //Direccion principal
        // [Required(ErrorMessage = "La direccion secundaria de residencia de usuario es requerido")]
        // public string? DirectionSec { get; set; } //Direccion Secundaria
        public string? Role { get; set; } // Se recibe el rol para poder asigarle un rol de los que se tiene en la base de datos
    }
}