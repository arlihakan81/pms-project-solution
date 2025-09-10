using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PMS.Application.Interfaces;
using PMS.Domain.Entities;
using PMS.Persistence.Context;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.Repositories
{
    public class AppUserRepository(AppDbContext context, IConfiguration config) : IAppUserRepository
    { 
        private readonly AppDbContext _context = context;
        private readonly IConfiguration _config = config;

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

        public string GenerateJwtToken(AppUser user)
        {
            var claims = new[]
            {
                new Claim("avatar", user.Avatar ?? string.Empty),
                new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
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
