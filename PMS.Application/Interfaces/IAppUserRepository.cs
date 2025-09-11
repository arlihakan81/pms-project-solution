using PMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.Interfaces
{
    public interface IAppUserRepository
    {
        Task<bool> IsEmailUniqueAsync(string email);
        Task<bool> IsEmailUniqueAsync(Guid userId, string email);
        Task AddUserAsync(AppUser appUser);
        Task UpdateUserAsync(AppUser appUser);
        Task DeleteUserAsync(Guid userId);
        Task<AppUser?> GetUserAsync(Guid userId);
        Task<List<AppUser>?> GetUsersAsync(Guid organizationId);
        Task<AppUser?> GetUserByEmailAsync(string email);
        Task<AppUser?> GetUserByTaskAsync(Guid taskId);
        Task<List<AppUser>?> GetUsersByProjectAsync(Guid projectId);

        string GenerateJwtToken(AppUser user);
    }
}
