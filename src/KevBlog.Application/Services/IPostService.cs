using KevBlog.Application.DTOs;
using KevBlog.Application.Common;
using KevBlog.Domain.Entities;

namespace KevBlog.Application.Services
{
    public interface IPostService
    {
        Task<IEnumerable<PostDisplayDto>> GetPosts();
        Task<IEnumerable<Post>> GetPostsByUsername(string username);
        Task<ServiceResult<PostDisplayDetailsDto>> GetByIdAsync(int id);
    }
}
