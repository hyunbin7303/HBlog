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
        public IQueryable<Message> GetMessagesQuery()
        {
            return _dbContext.Messages.OrderByDescending(x=> x.MessageSent).AsQueryable();
        }
        public async Task<Message> GetMessage(int id)
        {
            return await _dbContext.Messages.FindAsync(id);
        }

        public async Task<IEnumerable<Message>> GetMessageThread(string currentUsernename, string recipientUsername)
        {
            var messages = await _dbContext.Messages
                        .Include(x=> x.Sender).ThenInclude(p => p.Photos)
                        .Include(x=> x.Recipient).ThenInclude(p => p.Photos)
                        .Where(m => m.RecipientUsername ==currentUsernename 
                                 && m.SenderUsername == recipientUsername
                                 || m.RecipientUsername == recipientUsername
                                 && m.SenderUsername == currentUsernename)
                        .OrderBy(m => m.MessageSent)
                        .ToListAsync();
            
            var unreadMsgs = messages.Where(m => m.DateRead == null && m.RecipientUsername == currentUsernename).ToList();

            if(unreadMsgs.Any()){
                foreach(var msg in unreadMsgs){
                    msg.DateRead = DateTime.UtcNow;
                }
                await _dbContext.SaveChangesAsync();
            }
            return messages;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}