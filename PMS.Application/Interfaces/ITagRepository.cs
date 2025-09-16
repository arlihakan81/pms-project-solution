using PMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.Interfaces
{
    public interface ITagRepository
    {
        Task<Tag?> GetTagAsync(Guid tagId);
        Task<List<Tag>?> GetTagsByTaskAsync(Guid taskId);
        // Basic CRUD operations
        Task<List<Tag>?> GetTagsAsync(Guid organizationId);
        Task AddTagAsync(Tag tag);
        Task DeleteTagAsync(Guid tagId);
        Task UpdateTagAsync(Tag tag);

        // Basic logical methods e.g TagExists, GetTagsByName, etc. can be added here as needed.
        Task<bool> IsUniqueTagNameAsync(Guid organizationId, string name);
        Task<bool> IsUniqueTagNameAsync(Guid organizationId, string name, Guid excludeTagId);



    }
}
