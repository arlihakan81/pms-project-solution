using PMS.Domain.Common;
using PMS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Domain.Entities
{
    public class TaskItem : BaseEntity<Guid>
    {
        public Guid ProjectId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? DueDate { get; set; }
        public Enumeration.TaskStatus Status { get; set; } = Enumeration.TaskStatus.NotStarted;
        public Enumeration.PriorityLevel Priority { get; set; }
        public Guid? UserId { get; set; }

        public Project Project { get; set; }
        public AppUser? User { get; set; }

        public List<Tag>? Tags { get; set; }
        public List<Subtask>? Subtasks { get; set; }
    }
}
