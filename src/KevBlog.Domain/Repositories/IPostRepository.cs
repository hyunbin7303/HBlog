using KevBlog.Domain.Entities;

namespace KevBlog.Domain.Repositories
{
    public interface IPostRepository
    {
        Task CreateAsync(Post post);
        Task UpdateAsync(Post user);
        Task<IEnumerable<Post>> GetPostsAsync();
        Task<IEnumerable<Post>> GetPostsByUserName(string userName);
        Task<Post> GetPostById(int id);
        Task RemoveAsync(int id);
        Task AddTagInExistingPost(Post post, Tag tag);
    }
}