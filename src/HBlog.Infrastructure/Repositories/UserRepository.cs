using HBlog.Domain.Entities;
using HBlog.Domain.Repositories;
using HBlog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace HBlog.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IdentityContext _dbContext;
        public UserRepository(IdentityContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        public async Task<User> GetUserByIdAsync(Guid id)
        {
            return await _dbContext.Users.FindAsync(id);
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _dbContext.Users.Include(p => p.Photos).SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var normalized = email?.ToUpperInvariant();
            return await _dbContext.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.NormalizedEmail == normalized);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _dbContext.Users.Include(p=> p.Photos).AsNoTracking().ToListAsync();
        }
        public async Task<bool> SaveAllAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
        public void Update(User user)
        {
            _dbContext.Entry(user).State = EntityState.Modified;
        }

        public async Task<bool> SoftDeleteAsync(User user)
        {
            if (user is null) return false;
            if (user.IsDeleted) return true;

            var sentinel = $"deleted_{user.Id:N}";
            user.IsDeleted = true;
            user.DeletedAtUtc = DateTime.UtcNow;
            user.UserName = sentinel;
            user.NormalizedUserName = sentinel.ToUpperInvariant();
            user.Email = $"{sentinel}@invalid.local";
            user.NormalizedEmail = user.Email.ToUpperInvariant();
            user.FirstName = null;
            user.LastName = null;
            user.KnownAs = null;
            user.DateOfBirth = default;
            user.Gender = null;
            user.RefreshToken = null;
            user.PhoneNumber = null;
            user.PhoneNumberConfirmed = false;

            _dbContext.Entry(user).State = EntityState.Modified;
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}