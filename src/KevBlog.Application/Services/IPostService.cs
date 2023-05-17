using KevBlog.Application.DTOs;
using KevBlog.Application.Common;

namespace KevBlog.Application.Services
{
    public interface IPostService
    {
        Task<IEnumerable<PostDisplayDto>> GetPosts();
        Task<IEnumerable<PostDisplayDto>> GetPostsByTagName(string tagName);
        Task<ServiceResult<PostDisplayDetailsDto>> GetByIdAsync(int id);
        Task<ServiceResult> CreatePost(string userName, PostCreateDto createDto);
        Task<ServiceResult> UpdatePost(PostUpdateDto updateDto);
        Task<ServiceResult> AddTagForPost(int postId, int tagName);
    }
}
