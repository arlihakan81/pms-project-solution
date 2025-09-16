using PMS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Domain.Entities
{
    public class TaskTag : BaseEntity<Guid>
    {
        public Guid TaskItemId { get; set; }
        public Guid TagId { get; set; }

        public TaskItem TaskItem { get; set; }
        public Tag Tag { get; set; }
    }
}
