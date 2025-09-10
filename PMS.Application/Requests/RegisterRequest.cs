using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.Requests
{
    public class RegisterRequest : LoginRequest
    {
        [Required(ErrorMessage = "Adınız gerekli")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Soyadınız gerekli")]
        public string LastName { get; set; }

        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }


    }
}
