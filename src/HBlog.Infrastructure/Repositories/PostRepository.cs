using HBlog.Domain.Entities;
using HBlog.Domain.Repositories;
using HBlog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HBlog.Infrastructure.Repositories
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
        public async Task<IEnumerable<Post>> GetPostsAsync(int limit, int offset)
        {
            return await _dbContext.Posts.Include(p => p.PostTags)
                .AsNoTracking()
                .OrderByDescending(p => p.Created)
                .Skip(offset).Take(limit).ToListAsync();
        }
        public async Task UpdateAsync(Post user)
        {
            _dbContext.Entry(user).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Post> GetPostDetails(int id)
        {
            return await _dbContext.Posts
                .Where(p => p.Id == id)
                .Include(u => u.User)
                .Include(t => t.PostTags)
                .ThenInclude(o => o.Tag)
                .FirstOrDefaultAsync();
        }
    }
}