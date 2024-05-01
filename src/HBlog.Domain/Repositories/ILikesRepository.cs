using HBlog.Domain.Entities;
namespace HBlog.Domain.Repositories
{
    public interface ILikesRepository
    {
        Task<UserLike> GetUserLike(int srcUserId, int targetUserId);
        Task<User> GetUserWithLikes(int userId);
         
    }
}