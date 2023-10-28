using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using KevBlog.Infrastructure.Data;
using System;

namespace KevBlog.Infrastructure.Repositories
{
    public class FileStorageRepository : Repository<FileStorage>, IFileStorageRepository
    {
        private readonly DataContext _dbContext;
        public FileStorageRepository(DataContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Task<List<FileStorage>> GetAllFilesByUserIdAsync(string userId)
        {
            //_dbContext.FileStorages.Where(x => x.)
            throw new NotImplementedException();
        }
        public Task<bool> InsertDataAsync(string bucketName, string fileName, Stream fileStream)
        {
            throw new NotImplementedException();
        }
    }
}
