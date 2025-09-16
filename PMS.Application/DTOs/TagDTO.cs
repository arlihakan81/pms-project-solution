using PMS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.DTOs
{
    public class TagDTO : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string? Color { get; set; }
        public string? Icon { get; set; }
        public Guid OrganizationId { get; set; }
    }
}
