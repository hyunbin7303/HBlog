using HBlog.Contract.Common;
using HBlog.Contract.DTOs;
using HBlog.Domain.Common;
using HBlog.Domain.Entities;
using HBlog.Domain.Params;

namespace HBlog.Application.Services
{
    public interface IMessageService
    {
        Task<ServiceResult<MessageDto>> CreateMessage(string userName, MessageCreateDto createMsgDto);
        Task<PageList<MessageDto>> GetMessagesForUserPageList(MessageParams messageParams);
        Task<IEnumerable<MessageDto>> GetMessageThreads(string currUserName, string recipientUsername);
        Task<ServiceResult> DeleteMessage(string currUserName, int id);
        Task<Connection> GetConnection(string connId);

    }
}
