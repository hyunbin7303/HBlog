using HBlog.Domain.Entities;
using HBlog.Domain.Repositories;
using HBlog.Infrastructure.Data;
using System;

namespace HBlog.Infrastructure.Repositories
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
