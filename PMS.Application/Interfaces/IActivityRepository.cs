using PMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.Interfaces
{
    public interface IActivityRepository
    {
        Task<List<Activity>?> GetActivitiesByTaskAsync(Guid taskId);
        Task<List<Activity>?> GetActivitiesByUserAsync(Guid userId);
        Task<Activity?> GetActivityAsync(Guid activityId);
        Task AddActivityAsync(Activity activity);
        Task DeleteActivityAsync(Guid activityId);
        Task UpdateActivityAsync(Activity activity);

        Task<List<Activity>?> GetAllActivitiesAsync(Guid organizationId);
        Task<List<Activity>?> GetLast3ActivitiesAsync(Guid projectId);

        
    }
}
