using HBlog.Contract.Common;
using HBlog.Contract.DTOs;
namespace HBlog.Application.Services
{
    public interface ITagService
    {
        Task<IEnumerable<TagDto>> GetAllTags();
        Task<ServiceResult<IEnumerable<TagDto>>> GetTagsByPostId(int postId);
        ServiceResult CreateTag(TagCreateDto tag);
        Task<ServiceResult> RemoveTag(int tagId);

    }
}
