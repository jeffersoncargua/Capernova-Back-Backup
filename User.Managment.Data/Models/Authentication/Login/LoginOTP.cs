using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Managment.Data.Models.Authentication.Login
{
    public class LoginOTP
    {
        [Required(ErrorMessage ="El Email es requerido")]
        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage ="El codigo es requerido")]
        public string? OTP { get; set; }
    }
}
