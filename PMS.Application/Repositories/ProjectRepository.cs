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
    public class ProjectRepository(AppDbContext context) : IProjectRepository
    {
        private readonly AppDbContext _context = context;

        public async Task AddProjectAsync(Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
        }

        public async Task AssignToUserAsync(Guid projectId, Guid userId)
        {
            _context.Collaborations.Add(new Collaboration
            {
                ProjectId = projectId,
                UserId = userId
            });
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProjectAsync(Guid projectId)
        {
            _context.Projects.Remove(_context.Projects.FirstOrDefault(p => p.Id == projectId)!);
            await _context.SaveChangesAsync();
        }

        public async Task<Project?> GetProjectAsync(Guid projectId)
        {
            return await _context.Projects.Include(p => p.Tasks).Where(p => p.Id == projectId).FirstOrDefaultAsync();
        }

        public async Task<Project?> GetProjectByTitleAsync(string title)
        {
            return await _context.Projects.Include(p => p.Tasks).Where(p => p.Title == title).FirstOrDefaultAsync();
        }

        public async Task<List<Project>?> GetProjectsAsync(Guid organizationId)
        {
            return await _context.Projects.Include(p => p.Tasks).Where(p => p.OrganizationId == organizationId).ToListAsync();
        }

        public async Task<List<Project>?> GetProjectsByDateRangeAsync(Guid organizationId, DateTime startDate, DateTime endDate)
        {
            return await _context.Projects
                .Include(p => p.Tasks)
                .Where(p => p.OrganizationId == organizationId && p.StartDate >= startDate && p.Deadline <= endDate)
                .ToListAsync();
        }

        public async Task<List<Project>?> GetProjectsByUserAsync(Guid userId)
        {
            return await _context.Projects
                .Include(p => p.Tasks)
                .ThenInclude(t => t.User)
                .Where(p => p.Tasks.Any(t => t.UserId == userId))
                .ToListAsync();
        }

        public async Task<List<Project>?> GetUpcomingDeadlineProjectsAsync(Guid organizationId)
        {
            var currentDate = DateTime.Now;
            var upcomingDate = currentDate.AddDays(7);
            return await _context.Projects
                .Include(p => p.Tasks)
                .Where(p => p.OrganizationId == organizationId && p.Deadline >= currentDate && p.Deadline <= upcomingDate)
                .ToListAsync();
        }

        public async Task<bool> IsUniqueProjectTitleAsync(Guid organizationId, string title)
        {
            return await _context.Projects.AnyAsync(p => p.Title.ToLower().Trim() == title.ToLower().Trim() && p.OrganizationId == organizationId);
        }

        public async Task<bool> IsUniqueProjectTitleAsync(Guid projectId, Guid organizationId, string title)
        {
            return await _context.Projects.AnyAsync(p => p.Id != projectId  && p.Title.ToLower().Trim() == title.ToLower().Trim() && p.OrganizationId == organizationId);
        }

        public async Task UnassignFromUserAsync(Guid projectId, Guid userId)
        {
            var collaboration = _context.Collaborations.FirstOrDefault(c => c.ProjectId == projectId && c.UserId == userId);
            if (collaboration != null)
            {
                _context.Collaborations.Remove(collaboration);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateProjectAsync(Project project)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }
    }
}
