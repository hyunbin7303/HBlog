using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using KevBlog.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KevBlog.Infrastructure.Repositories
{
    public class FileStorageRepository : IFileStorageRepository
    {
        private readonly DataContext _dbContext;
        public FileStorageRepository(DataContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        public Task<FileStorage> CreateNewStorage(string bucketName)
        {
            throw new NotImplementedException();
        }

        public Task<FileStorage> GetbyIdAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
