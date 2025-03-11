
using HBlog.Domain.Entities;
using HBlog.Domain.Repositories;
using HBlog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HBlog.Infrastructure.Repositories
{
    public class LikesRepository : ILikesRepository
    {
        private readonly DataContext _dbContext;
        public LikesRepository(DataContext dbContext)
        {
            this._dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        public async Task<UserLike> GetUserLike(Guid srcUserId, Guid targetUserId)
        {
            return await _dbContext.Likes.FindAsync(srcUserId, targetUserId);
        }

        public async Task<User> GetUserWithLikes(Guid userId)
        {
            return await _dbContext.Users.Include(x=> x.LikedUsers).FirstOrDefaultAsync(x=> x.Id == userId);
        }
    }
}