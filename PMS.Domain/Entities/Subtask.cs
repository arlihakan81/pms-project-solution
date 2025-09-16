using PMS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Domain.Entities
{
    public class Subtask : BaseEntity<Guid>
    {
        public Guid TaskId { get; set; }
        public string Name { get; set; }
        public bool IsCompleted { get; set; } = false;

        public TaskItem TaskItem { get; set; }

    }
}
