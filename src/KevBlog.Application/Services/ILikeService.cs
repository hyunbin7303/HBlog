using KevBlog.Application.Common;
using KevBlog.Application.DTOs;
using KevBlog.Domain.Common;
using KevBlog.Infrastructure.Helpers;

namespace KevBlog.Application.Services
{
    public interface ILikeService
    {
        Task<PageList<LikeDto>> GetUserLikePageList(LikesParams likesParam);
        Task<ServiceResult> AddLike(int sourceUserId, string username);
    }
}
