using KevBlog.Domain.Entities;

namespace KevBlog.Domain.Interfaces
{
    public interface IPostRepository
    {
        Task CreateAsync(Post post);
        Task UpdateAsync(Post user);
        Task<IEnumerable<Post>> GetPostsAsync();
        Task<Post> GetPostByUsername(string username);
        Task<Post> GetPostById(int id);
        void Remove(int id);
    }
}