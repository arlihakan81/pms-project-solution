using Microsoft.EntityFrameworkCore;
using PMS.Application.Interfaces;
using PMS.Domain.Entities;
using PMS.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.Repositories
{
    public class ActivityRepository(AppDbContext context) : IActivityLogRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<List<ActivityLog>?> GetActivityLogsByTaskAsync(Guid taskId)
        {
            return await _context.ActivityLogs
                .Include(a => a.Task)
                .Include(a => a.User)
                .Where(a => a.TaskId == taskId).ToListAsync();
        }

        public async Task<List<ActivityLog>?> GetActivityLogsByUserAsync(Guid userId)
        {
            return await _context.ActivityLogs
                .Include(a => a.Task)
                .Include(a => a.User)
                .Where(a => a.UserId == userId).ToListAsync();
        }

        public async Task<ActivityLog?> GetActivityLogAsync(Guid activityId)
        {
            return await _context.ActivityLogs
                .Include(a => a.Task)
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == activityId);
        }

        public async Task<List<ActivityLog>?> GetAllActivityLogsAsync(Guid organizationId)
        {
            return await _context.ActivityLogs
                .Include(a => a.Task)
                .Include(a => a.User)
                .Where(a => a.Task.Project.OrganizationId == organizationId)
                .ToListAsync();
        }

        public async Task<List<ActivityLog>?> GetLast3ActivityLogsAsync(Guid projectId)
        {
            return await _context.ActivityLogs
                .Include(a => a.User)
                .Include(a => a.Task)
                .Where(a => a.Task.ProjectId == projectId)
                .OrderByDescending(a => a.CreatedAt)
                .Take(3)
                .ToListAsync();
        }

    }
}
