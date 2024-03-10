using KevBlog.Domain.Constants;
using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using KevBlog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace KevBlog.Infrastructure.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        private readonly DataContext _dbContext;
        public PostRepository(DataContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<Post>> GetPostsByUserName(string userName)
        {
            return await _dbContext.Posts.Where(x => x.User.UserName == userName).ToListAsync();
        }
        public async Task<IEnumerable<Post>> GetPostsAsync()
        {
            return await _dbContext.Posts.AsNoTracking().Include(u => u.User).ToListAsync();
        }
        public async Task UpdateAsync(Post user)
        {
            _dbContext.Entry(user).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
        public async Task<IEnumerable<Post>> GetPostsWithIncludesAsync(int id)
        {
            return await _dbContext.Posts.AsNoTracking().Include(x => x.Category).ToListAsync();
        }
    }
}