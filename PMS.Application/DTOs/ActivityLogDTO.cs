using PMS.Domain.Common;
using PMS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.DTOs
{
    public class ActivityLogDTO : BaseEntity<Guid>
    {
        public Guid UserId { get; set; }
        public Guid TaskId { get; set; }
    }
}
