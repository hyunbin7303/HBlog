using KevBlog.Contract.Common;
using KevBlog.Contract.DTOs;
using KevBlog.Domain.Common.Params;
namespace KevBlog.Application.Services;
public interface IPostService
{
    Task<IEnumerable<PostDisplayDto>> GetPosts(QueryParams query);
    Task<ServiceResult<IEnumerable<PostDisplayDto>>> GetPostsByTagName(string tagName); // (string tagName, int pageIndex, int pageSize, bool showHidden = false)etc...
    Task<ServiceResult<IEnumerable<PostDisplayDto>>> GetPostsByUsername(string userName);  
    Task<ServiceResult<PostDisplayDetailsDto>> GetByIdAsync(int id);
    Task<ServiceResult<PostDisplayDetailsDto>> GetBySlugAsync(string slug);
    Task<ServiceResult> CreatePost(string userName, PostCreateDto createDto);
    Task<ServiceResult> UpdatePost(PostUpdateDto updateDto);
    Task<ServiceResult> DeletePost(int id);
    Task<ServiceResult> AddTagForPost(int postId, int tagId);
    Task<ServiceResult<IEnumerable<PostDisplayDto>>> GetPostsByCategory(int categoryId);
}
