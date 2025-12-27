

namespace HBlog.Infrastructure.Services
{
    public interface IDataMigration
    {
        public Task<IEnumerable<string>> GetAppliedMigrationAsync(CancellationToken cancellationToken);
        public Task<IEnumerable<string>> GetPendingMigrationAsync(CancellationToken cancellationToken);
        public Task MigrateAsync(CancellationToken cancellationToken);
    }
}
