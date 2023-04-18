using KevBlog.Domain.Entities;

namespace KevBlog.Domain.Repositories
{
    public interface IUserRepository
    {
        Task Add(User user);
        void Update(User user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<User>> GetUsersAsync();
        IQueryable<User> GetUserQuery();
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByUsernameAsync(string username);
        IQueryable<User> GetUserLikesQuery(string predicate, int userId);
    }
}