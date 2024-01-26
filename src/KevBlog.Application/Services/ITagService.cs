using KevBlog.Application.Common;
using KevBlog.Contract.DTOs;

namespace KevBlog.Application.Services
{
    public interface ITagService
    {
        Task<IEnumerable<TagDto>> GetAllTags();
        Task<ServiceResult> AddTagToPost(int postId, string tagName);
        Task<ServiceResult> CreateTag(TagCreateDto tag);
        Task<ServiceResult> RemoveTag(int tagId);

    }
}
