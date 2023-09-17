using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using KevBlog.Infrastructure.Data;
using System;
namespace KevBlog.Infrastructure.Repositories
{
    public class FileStorageRepository : IFileStorageRepository
    {
        private readonly DataContext _dbContext;
        public FileStorageRepository(DataContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        public async Task<FileStorage> CreateNewStorage(string bucketName)
        {
            throw new NotImplementedException();
        }

        public Task<FileStorage> GetbyIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InsertDataAsync(string bucketName, string fileName, Stream fileStream)
        {
            throw new NotImplementedException();
        }
    }
}
