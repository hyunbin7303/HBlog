
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

        public async Task<IEnumerable<UserLike>> GetUserLikes(string predicate, int userId)
        {
            var users = _dbContext.Users.OrderBy(x => x.UserName).AsQueryable();
            var likes = _dbContext.Likes.AsQueryable();

            if(predicate == "liked"){
                likes = likes.Where(l => l.SourceUserId == userId);
                users = likes.Select(l => l.TargetUser);
            }
            if(predicate == "likedBy"){
                likes = likes.Where(l => l.TargetUserId == userId);
                users = likes.Select(l => l.SourceUser);
            }

            /*
                return await users.select(u => new LikeDto
                {
                    UserName = u.UserName,
                    KnownAs = u.KnownAs,
                    Age = u.DateOfBirth.CalculateAge(),
                    PhotoUrl = user.Photos.FirstOrDefault(x=> x.IsMain).Url,
                    City = user.City,
                    Id = user.id
                }).ToListAsync();

            */
            return await likes.ToListAsync();
        }

        public async Task<User> GetUserWithLikes(int userId)
        {
            return await _dbContext.Users.Include(x=> x.LikedUsers).FirstOrDefaultAsync(x=> x.Id == userId);
        }
    }
}