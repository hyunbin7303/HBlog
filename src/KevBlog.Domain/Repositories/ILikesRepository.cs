using KevBlog.Domain.Entities;
namespace KevBlog.Domain.Repositories
{
    public interface ILikesRepository
    {
        Task<UserLike> GetUserLike(int srcUserId, int targetUserId);
        Task<User> GetUserWithLikes(int userId);
        Task<IEnumerable<UserLike>> GetUserLikes(string predicate, int userId);
        
    }
}