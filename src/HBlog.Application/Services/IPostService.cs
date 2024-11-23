using HBlog.Contract.Common;
using HBlog.Contract.DTOs;
using HBlog.Domain.Common.Params;
namespace HBlog.Application.Services;
public interface IPostService
{
    Task<IEnumerable<PostDisplayDto>> GetPosts(PostParams query);
    Task<ServiceResult<IEnumerable<PostDisplayDto>>> GetPostsTitleContains(string title);
    Task<ServiceResult<IEnumerable<PostDisplayDto>>> GetPostsByTagSlug(string tagSlug); // (string tagName, int pageIndex, int pageSize, bool showHidden = false)etc...
    Task<ServiceResult<IEnumerable<PostDisplayDto>>> GetPostsByTagId(int tagId);
    Task<ServiceResult<IEnumerable<PostDisplayDto>>> GetPostsByUsername(string userName);  
    Task<ServiceResult<PostDisplayDetailsDto>> GetByIdAsync(int id);
    Task<ServiceResult<PostDisplayDetailsDto>> GetBySlugAsync(string slug);
    Task<ServiceResult> CreatePost(string userName, PostCreateDto createDto);
    Task<ServiceResult> UpdatePost(PostUpdateDto updateDto);
    Task<ServiceResult> UpdateStatus(int id, PostChangeStatusDto  updateStatusDto);
    Task<ServiceResult> DeletePost(int id);
    Task<ServiceResult> AddTagForPost(int postId, int[] tagIds);
    Task<ServiceResult<IEnumerable<PostDisplayDto>>> GetPostsByCategory(int categoryId);
}
