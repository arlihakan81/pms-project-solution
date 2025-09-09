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
    public class AppUserRepository(AppDbContext context) : IAppUserRepository
    { 
        private readonly AppDbContext _context = context;

        public async Task AddUserAsync(AppUser appUser)
        {
            _context.Users.Add(appUser);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            _context.Users.Remove(_context.Users.FirstOrDefault(u => u.Id == userId)!);
            await _context.SaveChangesAsync();
        }

        public async Task<AppUser?> GetUserAsync(Guid userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<AppUser?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<AppUser?> GetUserByTaskAsync(Guid taskId)
        {
            return await _context.Users.Include(u => u.Tasks).FirstOrDefaultAsync(u => u.Tasks.Any(t => t.Id == taskId));
        }

        public async Task<List<AppUser>?> GetUsersAsync()
        {
            return await _context.Users.Include(u => u.Tasks).ToListAsync();
        }

        public async Task<List<AppUser>?> GetUsersByProjectAsync(Guid projectId)
        {
            return await _context.Users.Include(u => u.Projects).Where(u => u.Projects.Any(p => p.Id == projectId)).ToListAsync();
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email.ToLower().Trim() == email.ToLower().Trim());
        }

        public async Task<bool> IsEmailUniqueAsync(Guid userId, string email)
        {
            return await _context.Users.AnyAsync(u => u.Id != userId && u.Email.ToLower().Trim() == email.ToLower().Trim());
        }

        public async Task UpdateUserAsync(AppUser appUser)
        {
            _context.Users.Update(appUser);
            await _context.SaveChangesAsync();
        }
    }
}
