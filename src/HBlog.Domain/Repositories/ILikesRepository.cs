using HBlog.Domain.Entities;
namespace HBlog.Domain.Repositories
{
    public interface ILikesRepository
    {
        Task<UserLike> GetUserLike(Guid srcUserId, Guid targetUserId);
        Task<User> GetUserWithLikes(Guid userId);
         
    }
}