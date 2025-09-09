using PMS.Domain.Common;
using PMS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Domain.Entities
{
    public class AppUser : BaseEntity<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public Enumeration.AppRole Role { get; set; } = Enumeration.AppRole.Member;
        public List<TaskItem> Tasks { get; set; }
        public List<Project> Projects { get; set; }
    }
}
