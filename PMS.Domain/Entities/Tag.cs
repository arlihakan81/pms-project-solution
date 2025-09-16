using PMS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Domain.Entities
{
    public class Tag : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string? Color { get; set; }
        public string? Icon { get; set; }
        public Guid OrganizationId { get; set; }

        public List<TaskItem>? Tasks { get; set; }
        public Organization Organization { get; set; }

    }
}
