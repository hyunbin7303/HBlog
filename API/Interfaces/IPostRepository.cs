using API.Entities;
namespace API.Interfaces
{
    public interface IPostRepository
    {
        void Update(User user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<Post>> GetPostsAsync();
        Task<User> GetPostByUsername(string username);
    }
}