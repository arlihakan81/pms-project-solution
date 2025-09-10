using PMS.Domain.Common;
using PMS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.DTOs
{
    public class AppUserDTO : BaseEntity<Guid>
    {
        public string? Avatar { get; set; }
        [Required(ErrorMessage = "Adınız gerekli")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Soyadınız gerekli")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "E-posta adresiniz gerekli")]
        public string Email { get; set; }
        public string Password { get; set; }
        public Enumeration.AppRole Role { get; set; } = Enumeration.AppRole.Member;
    }
}
