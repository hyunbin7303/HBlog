using KevBlog.Contract.Common;
using KevBlog.Contract.DTOs;
namespace KevBlog.Application.Services
{
    public interface ITagService
    {
        Task<IEnumerable<TagDto>> GetAllTags();
        Task<ServiceResult<IEnumerable<TagDto>>> GetTagsByPostId(int postId);
        Task<ServiceResult> CreateTag(TagCreateDto tag);
        Task<ServiceResult> RemoveTag(int tagId);

    }
}
