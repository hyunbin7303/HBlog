using HBlog.Contract.Common;
using HBlog.Contract.DTOs;
using HBlog.Domain.Common;
using HBlog.Domain.Params;

namespace HBlog.Application.Services
{
    public interface ILikeService
    {
        Task<PageList<LikeDto>> GetUserLikePageList(LikesParams likesParam);
        Task<ServiceResult> AddLike(Guid sourceUserId, string username);
    }
}
