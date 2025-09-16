using PMS.Domain.Common;

namespace PMS.Domain.Entities
{
    public class Comment : BaseEntity<Guid>
    {
        public Guid TaskItemId { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; }

        public TaskItem TaskItem { get; set; }
        public AppUser User { get; set; }
    }
}