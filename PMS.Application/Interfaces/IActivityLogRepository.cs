using PMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.Interfaces
{
    public interface IActivityLogRepository
    {
        Task<List<ActivityLog>?> GetActivityLogsByTaskAsync(Guid taskId);
        Task<List<ActivityLog>?> GetActivityLogsByUserAsync(Guid userId);
        Task<ActivityLog?> GetActivityLogAsync(Guid activityLogId);

        Task<List<ActivityLog>?> GetAllActivityLogsAsync(Guid organizationId);
        Task<List<ActivityLog>?> GetLast3ActivityLogsAsync(Guid projectId);

        
    }
}
