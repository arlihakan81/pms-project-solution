using PMS.Domain.Common;
using PMS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.DTOs
{
    public class ProjectDTO : BaseEntity<Guid>
    {
        public string? CoverImageUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime Deadline { get; set; }
        public decimal Budget { get; set; }
        public Enumeration.ProjectStatus Status { get; set; } = Enumeration.ProjectStatus.Inactive;
        public Enumeration.Privacy Privacy { get; set; } = Enumeration.Privacy.Private;
        public string? Progress { get; set; }
        public string? Percentage { get; set; }
        public Guid OrganizationId { get; set; }
    }
}
