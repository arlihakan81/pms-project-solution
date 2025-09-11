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
    public class TaskRepository(AppDbContext context) : ITaskRepository
    {
        private readonly AppDbContext _context = context;

        public async Task AddTaskAsync(TaskItem task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTaskAsync(Guid taskId)
        {
            _context.Tasks.Remove(_context.Tasks.FirstOrDefault(t => t.Id == taskId)!);
            await _context.SaveChangesAsync();
        }

        public async Task<List<TaskItem>?> GetOverdueTasksAsync(Guid organizationId)
        {
            var currentDate = DateTime.UtcNow;
            return await _context.Tasks
                .Include(t => t.Project)
                .Where(t => t.Project.OrganizationId == organizationId && t.EndDate < currentDate && t.Status != Domain.Enums.Enumeration.TaskStatus.Completed)
                .ToListAsync();
        }

        public async Task<TaskItem?> GetTaskAsync(Guid taskId)
        {
            return await _context.Tasks.FindAsync(taskId);
        }

        public async Task<TaskItem?> GetTaskByTitleAsync(string title)
        {
            return await _context.Tasks.Where(t => t.Title == title).FirstOrDefaultAsync();
        }

        public async Task<List<TaskItem>?> GetTasksAsync(Guid projectId)
        {
            return await _context.Tasks.Where(t => t.ProjectId == projectId).ToListAsync();
        }

        public async Task<bool> IsUniqueTaskTitleAsync(Guid projectId, string title)
        {
            return await _context.Tasks.AnyAsync(t => t.Title.ToLower().Trim() == title.ToLower().Trim() && t.ProjectId == projectId);
        }

        public async Task<bool> IsUniqueTaskTitleAsync(Guid taskId, Guid projectId, string title)
        {
            return await _context.Tasks.AnyAsync(t => t.Id != taskId && t.Title.ToLower().Trim() == title.ToLower().Trim() && t.ProjectId == projectId);
        }

        public async Task UpdateTaskAsync(TaskItem task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }
    }
}
