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
        public int DurationInDays { get; set; }
        public DateTime EndDate => StartDate.AddDays(DurationInDays);
        public Enumeration.TaskStatus Status { get; set; } = Enumeration.TaskStatus.NotStarted;
        public Enumeration.PriorityLevel Priority { get; set; }
        public Guid AssigneeId { get; set; }

        public Project Project { get; set; }
        public AppUser User { get; set; }
    }
}
