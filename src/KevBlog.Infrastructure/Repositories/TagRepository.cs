using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using KevBlog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace KevBlog.Infrastructure.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly DataContext _dbContext;
        public TagRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task Insert(string tagName, string tagDesc = "", string tagSlug = "")
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(int id, string tagName, string tagDesc, string tagSlug)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Tag>> GetAll()
        {
            return await _dbContext.Tags.AsNoTracking().ToListAsync();
        }

        public Task<IEnumerable<Tag>> FindbyTagName(string tagName)
        {
            throw new NotImplementedException();
        }
    }
}