using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Managment.Data.Models.Authentication.SignUp
{
    public class RegisterUser
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido ")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El apellido de usuario es requerido ")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "El correo del usuario es requerido ")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña de usuario es requerido ")]
        public string Password { get; set; }

        [Required(ErrorMessage = "La confirmación de contraseña de usuario es requerido ")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "El teléfono de usuario es requerido ")]
        [StringLength(10, ErrorMessage = "Deben ser 10 digitos")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "La cuidad de residencia de usuario es requerido")]
        public string City { get; set; }

    }
}
