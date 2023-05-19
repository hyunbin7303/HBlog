using KevBlog.Application.Common;
using KevBlog.Application.DTOs;
using KevBlog.Domain.Common;
using KevBlog.Domain.Params;

namespace KevBlog.Application.Services
{
    public interface IMessageService
    {
        Task<ServiceResult<MessageDto>> CreateMessage(string userName, MessageCreateDto createMsgDto);
        Task<PageList<MessageDto>> GetMessagesForUserPageList(MessageParams messageParams);
        Task<IEnumerable<MessageDto>> GetMessageThreads(string currUserName, string recipientUsername);
        Task<ServiceResult> DeleteMessage(string currUserName, int id);
    }
}
