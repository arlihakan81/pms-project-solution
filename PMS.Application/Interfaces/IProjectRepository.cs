using PMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.Interfaces
{
    public interface IProjectRepository
    {
        Task<Project?> GetProjectByTitleAsync(string title);
        Task<Project?> GetProjectAsync(Guid projectId);
        Task<List<Project>?> GetProjectsByUserAsync(Guid userId);
        Task<List<Project>?> GetUpcomingDeadlineProjectsAsync(Guid organizationId);
        Task AssignToUserAsync(Guid projectId, Guid userId);
        Task UnassignFromUserAsync(Guid projectId, Guid userId);
        // Basic CRUD Operations
        Task<List<Project>?> GetProjectsAsync(Guid organizationId);
        Task<List<Project>?> GetProjectsByDateRangeAsync(Guid organizationId, DateTime startDate, DateTime endDate);
        Task AddProjectAsync(Project project);
        Task UpdateProjectAsync(Project project);
        Task DeleteProjectAsync(Guid projectId);

        // Logical methods ...(e.g., checking for unique titles)
        Task<bool> IsUniqueProjectTitleAsync(Guid organizationId, string title);
        Task<bool> IsUniqueProjectTitleAsync(Guid projectId, Guid organizationId, string title);
    }
}
