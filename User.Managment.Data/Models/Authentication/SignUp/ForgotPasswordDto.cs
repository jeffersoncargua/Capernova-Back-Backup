// <copyright file="ForgotPasswordDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace User.Managment.Data.Models.Authentication.SignUp
{
    public class ForgotPasswordDto
    {
        [Required(ErrorMessage = "El correo es requerido")]

        // [DataType(DataType.EmailAddress)]
        // [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "El correo no tiene el formato adecuado")]
        public string? Email { get; set; }
    }
}