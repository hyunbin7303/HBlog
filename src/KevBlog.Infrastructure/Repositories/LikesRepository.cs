
using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using KevBlog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace KevBlog.Infrastructure.Repositories
{
    public class LikesRepository : ILikesRepository
    {
        private readonly DataContext _dbContext;
        public LikesRepository(DataContext dbContext)
        {
            this._dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        public async Task<UserLike> GetUserLike(int srcUserId, int targetUserId)
        {
            return await _dbContext.Likes.FindAsync(srcUserId, targetUserId);
        }

        public async Task<User> GetUserWithLikes(int userId)
        {
            return await _dbContext.Users.Include(x=> x.LikedUsers).FirstOrDefaultAsync(x=> x.Id == userId);
        }
    }
}