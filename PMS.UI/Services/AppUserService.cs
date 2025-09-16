using PMS.Domain.Entities;
using PMS.Persistence.Context;

namespace PMS.UI.Services
{
    public class AppUserService(AppDbContext context)
    {
        private readonly AppDbContext _context = context;

        public List<AppUser>? GetUsersByProject(Guid projectId)
        {
            return [.. _context.Users.Where(u => u.Projects.Any(p => p.Id == projectId))];
        }

    }
}
