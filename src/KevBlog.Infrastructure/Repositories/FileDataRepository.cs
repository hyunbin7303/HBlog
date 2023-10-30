using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using KevBlog.Infrastructure.Data;
using System;

namespace KevBlog.Infrastructure.Repositories
{
    public class FileDataRepository : Repository<FileData>, IFileDataRepository
    {
        private readonly DataContext _dbContext;
        public FileDataRepository(DataContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
    }
}
