using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Managment.Data.Models.Authentication.Login
{
    public class LoginModel
    {
        [Required(ErrorMessage ="El correo de usuario es requerido")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage ="La constraseña es requerida")]
        public string Password { get; set; }

    }
}
