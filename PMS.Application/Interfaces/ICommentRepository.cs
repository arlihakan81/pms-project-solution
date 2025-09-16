using PMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.Interfaces
{
    public interface ICommentRepository
    {
        Task<Comment?> GetCommentAsync(Guid commentId);
        Task<List<Comment>?> GetCommentByUserAsync(Guid userId);
        Task<List<Comment>?> GetCommentsByTaskAsync(Guid taskId);


        Task AddCommentAsync(Comment comment);
        Task DeleteCommentAsync(Guid commentId);
        Task UpdateCommentAsync(Comment comment);





    }
}
