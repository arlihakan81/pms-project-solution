using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.Requests
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "E-posta adresiniz gerekli")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Parolanız gerekli")]
        public string Password { get; set; }
    }
}
