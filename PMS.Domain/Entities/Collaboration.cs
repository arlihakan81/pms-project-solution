using PMS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Domain.Entities
{
    public class Collaboration : BaseEntity<Guid>
    {
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }

        public Project Project { get; set; }
        public AppUser User { get; set; }

    }
}
