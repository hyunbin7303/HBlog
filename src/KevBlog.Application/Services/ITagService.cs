using KevBlog.Application.Common;

namespace KevBlog.Application.Services
{
    public interface ITagService
    {
        Task<ServiceResult> AddTagToPost(int postId, string tagName);
        Task<ServiceResult> CreateTag(string tagName);
        Task<ServiceResult> RemoveTag(int tagId);

    }
}
