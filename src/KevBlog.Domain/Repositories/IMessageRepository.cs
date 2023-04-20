using KevBlog.Domain.Entities;

namespace KevBlog.Domain.Repositories
{
    public interface IMessageRepository
    {
        void AddMessage(Message msg);
        void DeleteMessage(Message msg);
        Task<Message> GetMessage(int id);
        // Task<PagedList<Message>> GetMessagesForUser();
        Task<IEnumerable<Message>> GetMessageThread(int currUserId, int recipientId);
        Task<bool> SaveAllAsync();
    }
}