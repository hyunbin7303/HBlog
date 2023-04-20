using KevBlog.Domain.Entities;

namespace KevBlog.Domain.Repositories
{
    public interface IMessageRepository
    {
        void AddMessage(Message msg);
        void DeleteMessage(Message msg);
        IQueryable<Message> GetMessagesQuery();
        Task<Message> GetMessage(int id);
        Task<IEnumerable<Message>> GetMessageThread(string currentUsernename, string recipientUsername);
        Task<bool> SaveAllAsync();
    }
}