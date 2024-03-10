using KevBlog.Contract.Common;
using KevBlog.Contract.DTOs;
using KevBlog.Domain.Common;
using KevBlog.Domain.Params;

namespace KevBlog.Application.Services
{
    public interface ILikeService
    {
        Task<PageList<LikeDto>> GetUserLikePageList(LikesParams likesParam);
        Task<ServiceResult> AddLike(int sourceUserId, string username);
    }
}
