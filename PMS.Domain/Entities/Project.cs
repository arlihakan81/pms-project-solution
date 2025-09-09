using PMS.Domain.Common;
using PMS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Domain.Entities
{
    public class Project : BaseEntity<Guid>
    {
        public string? CoverImageUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime Deadline { get; set; }
        public decimal Budget { get; set; } 
        public Enumeration.ProjectStatus Status { get; set; } = Enumeration.ProjectStatus.Inactive;
        public Enumeration.Privacy Privacy { get; set; } = Enumeration.Privacy.Private;

        public List<TaskItem> Tasks { get; set; } = new List<TaskItem>();


    }
}
