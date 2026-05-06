
using HBlog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HBlog.Infrastructure.Services
{
    public class IdentityMigration(IdentityContext _context)
    {
       public Task<IEnumerable<string>> GetAppliedMigrationAsync(CancellationToken cancellationToken)
            => _context.Database.GetAppliedMigrationsAsync(cancellationToken);
        public Task<IEnumerable<string>> GetPendingMigrationAsync(CancellationToken cancellationToken)
            => _context.Database.GetPendingMigrationsAsync(cancellationToken);
        public Task MigrateAsync(CancellationToken cancellationToken)
            => _context.Database.MigrateAsync(cancellationToken);
    }
    
    public class BlogMigration(BlogContext _context)
    {
       public Task<IEnumerable<string>> GetAppliedMigrationAsync(CancellationToken cancellationToken)
            => _context.Database.GetAppliedMigrationsAsync(cancellationToken);
        public Task<IEnumerable<string>> GetPendingMigrationAsync(CancellationToken cancellationToken)
            => _context.Database.GetPendingMigrationsAsync(cancellationToken);
        public Task MigrateAsync(CancellationToken cancellationToken)
            => _context.Database.MigrateAsync(cancellationToken);
    }
}