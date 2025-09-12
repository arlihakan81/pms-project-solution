using PMS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Domain.Entities
{
    public class Activity : BaseEntity<Guid>
    {
        public Guid UserId { get; set; }
        public Guid TaskId { get; set; }
        public Guid? CommentId { get; set; }

        public AppUser User { get; set; }
        public TaskItem Task { get; set; }
        public Comment? Comment { get; set; }

    }
}
