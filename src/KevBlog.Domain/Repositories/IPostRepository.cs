using KevBlog.Domain.Entities;

namespace KevBlog.Domain.Repositories
{
    public interface IPostRepository : IRepository<Post>
    {
        Task UpdateAsync(Post user);
        Task<IEnumerable<Post>> GetPostsAsync();
        Task<IEnumerable<Post>> GetPostsByUserName(string userName);
    }
}