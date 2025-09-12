using PMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.Interfaces
{
    public interface ITaskRepository
    {
        Task<TaskItem?> GetTaskByTitleAsync(string title);
        Task<TaskItem?> GetTaskAsync(Guid taskId);
        Task<List<TaskItem>?> GetOverdueTasksAsync(Guid organizationId);
        Task<List<TaskItem>?> GetUnassignedTasksAsync(Guid projectId);
        // Basic CRUD Operations
        Task<List<TaskItem>?> GetTasksAsync(Guid projectId);
        Task<List<TaskItem>?> GetTasksByUserAsync(Guid userId);
        Task AddTaskAsync(TaskItem task);
        Task UpdateTaskAsync(TaskItem task);
        Task DeleteTaskAsync(Guid taskId);

        // Logical methods ...(e.g., checking for unique titles)
        Task<bool> IsUniqueTaskTitleAsync(Guid projectId, string title);
        Task<bool> IsUniqueTaskTitleAsync(Guid taskId, Guid projectId, string title);
    }
}
