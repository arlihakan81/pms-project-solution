using PMS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.DTOs
{
    public class CommentDTO : BaseEntity<Guid>
    {
        public Guid TaskId { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; }
    }
}
