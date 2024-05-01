using HBlog.Domain.Entities;

namespace HBlog.Domain.Repositories
{
    public interface IMessageRepository
    {
        void AddMessage(Message msg);
        void DeleteMessage(Message msg);
        Task<IEnumerable<Message>> GetMessages();
        Task<Message> GetMessage(int id);
        Task<IEnumerable<Message>> GetMessageThread(string currentUsernename, string recipientUsername);
        Task<bool> SaveAllAsync();
    }
}