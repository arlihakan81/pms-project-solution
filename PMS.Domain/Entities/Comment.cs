using PMS.Domain.Common;

namespace PMS.Domain.Entities
{
    public class Comment : BaseEntity<Guid>
    {
        public string Content { get; set; }
        public List<Activity>? Activities { get; set; }
    }
}