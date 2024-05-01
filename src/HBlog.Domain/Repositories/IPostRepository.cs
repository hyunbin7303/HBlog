using HBlog.Domain.Entities;

namespace HBlog.Domain.Repositories
{
    public interface IPostRepository : IRepository<Post>
    {
        Task UpdateAsync(Post post);
        Task<IEnumerable<Post>> GetPostsAsync();
        Task<Post> GetPostDetails(int id); 
        Task<IEnumerable<Post>> GetPostsAsync(int limit, int offset);   
        Task<IEnumerable<Post>> GetPostsByUserName(string userName);
    }
}