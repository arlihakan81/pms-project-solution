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
        public string Progress => $"{Tasks.Count}/{Tasks.Count(t => t.Status == Enumeration.TaskStatus.Completed)}"; 
        public string Percentage => Tasks.Count == 0 ? "0%" : $"{(int)((Tasks.Count(t => t.Status == Enumeration.TaskStatus.Completed) / (double)Tasks.Count) * 100)}%";

        public List<TaskItem> Tasks { get; set; } = new List<TaskItem>();
        public List<AppUser> Collaborators { get; set; }

    }
}
