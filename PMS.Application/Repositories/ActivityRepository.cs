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
    public class ActivityRepository(AppDbContext context) : IActivityRepository
    {
        private readonly AppDbContext _context = context;

        public async Task AddActivityAsync(Activity activity)
        {
            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteActivityAsync(Guid activityId)
        {
            var activity = await _context.Activities.FindAsync(activityId);
            if (activity != null)
            {
                _context.Activities.Remove(activity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Activity>?> GetActivitiesByTaskAsync(Guid taskId)
        {
            return await _context.Activities
                .Include(a => a.Task)
                .Include(a => a.User)
                .Where(a => a.TaskId == taskId).ToListAsync();
        }

        public async Task<List<Activity>?> GetActivitiesByUserAsync(Guid userId)
        {
            return await _context.Activities
                .Include(a => a.Task)
                .Include(a => a.User)
                .Include(a => a.Comment).Where(a => a.UserId == userId).ToListAsync();
        }

        public async Task<Activity?> GetActivityAsync(Guid activityId)
        {
            return await _context.Activities
                .Include(a => a.Task)
                .Include(a => a.User)
                .Include(a => a.Comment)
                .FirstOrDefaultAsync(a => a.Id == activityId);
        }

        public async Task<List<Activity>?> GetAllActivitiesAsync(Guid organizationId)
        {
            return await _context.Activities
                .Include(a => a.Task)
                .Include(a => a.User)
                .Include(a => a.Comment)
                .Where(a => a.Task.Project.OrganizationId == organizationId)
                .ToListAsync();
        }

        public async Task<List<Activity>?> GetLast3ActivitiesAsync(Guid projectId)
        {
            return await _context.Activities
                .Include(a => a.User)
                .Include(a => a.Comment)
                .Include(a => a.Task)
                .Where(a => a.Task.ProjectId == projectId)
                .OrderByDescending(a => a.CreatedAt)
                .Take(3)
                .ToListAsync();
        }

        public async Task UpdateActivityAsync(Activity activity)
        {
            _context.Activities.Update(activity);
            await _context.SaveChangesAsync();
        }
    }
}
