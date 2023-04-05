using KevBlog.Domain.Entities;

namespace API.Interfaces
{
    public interface IPostRepository
    {
        void Update(Post user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<Post>> GetPostsAsync();
        Task<Post> GetPostByUsername(string username);
        Task<Post> GetPostById(int id);
        void Remove(int id);
    }
}