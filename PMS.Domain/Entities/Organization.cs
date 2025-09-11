using PMS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Domain.Entities
{
    public class Organization : BaseEntity<Guid>
    {
        public string Name { get; set; }

        public List<AppUser> Users { get; set; }
        public List<Project> Projects { get; set; }
    }
}
