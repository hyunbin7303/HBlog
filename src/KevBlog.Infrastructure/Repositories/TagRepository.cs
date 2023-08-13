using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using KevBlog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace KevBlog.Infrastructure.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly DataContext _dbContext;
        public TagRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Insert(Tag tag)
        {
            _dbContext.Tags.Add(tag);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(Tag tag)
        {
            _dbContext.Entry(tag).State = EntityState.Deleted;
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(Tag tag)
        {
            _dbContext.Entry(tag).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Tag>> GetAll()
        {
            return await _dbContext.Tags.AsNoTracking().ToListAsync();
        }

        public IEnumerable<Tag> FindbyTagName(string tagName)
        {
            return _dbContext.Tags.Where(x => x.Name == tagName).AsEnumerable();
        }

        public async Task<Tag> GetById(int id)
        {
            return await _dbContext.Tags.FindAsync(id);
        }
    }
}