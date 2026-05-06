using HBlog.Domain.Entities;
using HBlog.Domain.Repositories;
using HBlog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HBlog.Infrastructure.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        private readonly BlogContext _dbContext;
        public PostRepository(BlogContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<Post>> GetPostsByUserId(Guid userId)
        {
            return await _dbContext.Posts
                .Where(p => p.UserId == userId)
                .Include(p => p.Tags)       
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetPostsByUserName(string userName)
        {
            // Note: Cannot use navigation property across contexts. 
            // Use GetPostsByUserId at application layer after fetching user by username
            throw new NotImplementedException("Use GetPostsByUserId instead - coordinate at service layer");
        }
        
        public async Task<IEnumerable<Post>> GetPostsAsync()
        {
            // Removed .Include(u => u.User) as User is in different context
            return await _dbContext.Posts.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetPostsTitleContainsAsync(string searchTitle)
        {
            return await _dbContext.Posts.Where(p => p.Title.ToString().ToLower().Contains(searchTitle)).ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetPostsAsync(int limit, int offset)
        {
            return await _dbContext.Posts.Include(p => p.Tags)
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
            // Removed .Include(p => p.User) as User is in different context
            return await _dbContext.Posts
                .Where(p => p.Id == id)
                .Include(t => t.Tags)
                .FirstOrDefaultAsync();
        }
    }
}