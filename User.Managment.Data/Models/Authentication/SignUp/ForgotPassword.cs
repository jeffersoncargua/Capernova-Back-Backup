using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Managment.Data.Models.Authentication.SignUp
{
    public class ForgotPassword
    {
        [Required(ErrorMessage ="El correo es requerido")]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
