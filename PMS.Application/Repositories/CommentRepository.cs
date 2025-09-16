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
    public class CommentRepository(AppDbContext context) : ICommentRepository
    {
        private readonly AppDbContext _context = context;

        public async Task AddCommentAsync(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCommentAsync(Guid commentId)
        {
            var comment = _context.Comments.Find(commentId);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Comment?> GetCommentAsync(Guid commentId)
        {
            return await _context.Comments.FindAsync(commentId);
        }

        public async Task<List<Comment>?> GetCommentByUserAsync(Guid userId)
        {
            return await _context.Comments
                .Where(a => a.UserId == userId).ToListAsync();
        }

        public async Task<List<Comment>?> GetCommentsByTaskAsync(Guid taskId)
        {
            return await _context.Comments
                .Where(a => a.TaskItemId == taskId).ToListAsync();
        }

        public async Task UpdateCommentAsync(Comment comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
        }
    }
}
