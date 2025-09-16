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
    public class TagRepository(AppDbContext context) : ITagRepository
    {
        private readonly AppDbContext _context = context;

        public async Task AddTagAsync(Tag tag)
        {
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTagAsync(Guid tagId)
        {
            var tag = _context.Tags.FirstOrDefault(t => t.Id == tagId);
            if (tag != null)
            {
                _context.Tags.Remove(tag);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Tag?> GetTagAsync(Guid tagId)
        {
            return await _context.Tags.Include(tg => tg.Tasks).FirstOrDefaultAsync(t => t.Id == tagId);
        }

        public async Task<List<Tag>?> GetTagsAsync(Guid organizationId)
        {
            return await _context.Tags.Include(tg => tg.Tasks).Where(t => t.OrganizationId == organizationId).ToListAsync();
        }

        public async Task<List<Tag>?> GetTagsByTaskAsync(Guid taskId)
        {
            return await _context.TaskTags
                .Where(tt => tt.TaskItemId == taskId)
                .Include(tt => tt.Tag)
                .Include(tt => tt.TaskItem)
                .Select(tt => tt.Tag)
                .ToListAsync();
        }

        public async Task<bool> IsUniqueTagNameAsync(Guid organizationId, string name)
        {
            return await _context.Tags.AnyAsync(t => t.OrganizationId == organizationId && t.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public async Task<bool> IsUniqueTagNameAsync(Guid organizationId, string name, Guid excludeTagId)
        {
            return await _context.Tags.AnyAsync(t => t.OrganizationId == organizationId && t.Id != excludeTagId && t.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public async Task UpdateTagAsync(Tag tag)
        {
            _context.Tags.Update(tag);
            await _context.SaveChangesAsync();
        }
    }
}
