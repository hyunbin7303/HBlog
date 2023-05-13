using KevBlog.Application.DTOs;
using KevBlog.Application.Common;

namespace KevBlog.Application.Services
{
    public interface IPostService
    {
        Task<IEnumerable<PostDisplayDto>> GetPosts();
        Task<ServiceResult<PostDisplayDetailsDto>> GetByIdAsync(int id);
        Task<ServiceResult> CreatePost(string userName, PostCreateDto createDto);
        Task AddTag(string tagName);
    }
}
