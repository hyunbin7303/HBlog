using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; 
using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using KevBlog.Infrastructure.Data;

namespace KevBlog.Infrastructure.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _dbContext;
        public MessageRepository(DataContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public void AddMessage(Message msg)
        {
            _dbContext.Messages.Add(msg);
        }
        public void DeleteMessage(Message msg)
        {
            _dbContext.Messages.Remove(msg);
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _dbContext.Messages.FindAsync(id);
        }

        public Task<IEnumerable<Message>> GetMessageThread(int currUserId, int recipientId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}