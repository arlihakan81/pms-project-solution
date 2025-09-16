using PMS.Domain.Entities;
using PMS.Persistence.Context;

namespace PMS.UI.Services
{
    public class TaskService(AppDbContext context)
    {
        private readonly AppDbContext _context = context;

        public List<TaskItem>? GetTasksByProject(Guid projectId)
        {
            return [.. _context.Tasks.Where(t => t.ProjectId == projectId)];
        }


    }
}
