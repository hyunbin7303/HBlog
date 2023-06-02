using AutoMapper;
using KevBlog.Application.Common;
using KevBlog.Application.DTOs;
using KevBlog.Domain.Common;
using KevBlog.Domain.Entities;
using KevBlog.Domain.Params;
using KevBlog.Domain.Repositories;

namespace KevBlog.Application.Services
{
    public class MessageService : BaseService,  IMessageService
    {
        private readonly IMessageRepository _msgRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGroupRepository _groupRepository;
        public MessageService(IMapper mapper,IMessageRepository messageRepository,IUserRepository userRepository, IGroupRepository groupRepository) : base(mapper)
        {
            _msgRepository = messageRepository;
            _userRepository = userRepository;
            _groupRepository = groupRepository;
        }
        public async Task<ServiceResult<MessageDto>> CreateMessage(string userName, MessageCreateDto createMsgDto)
        {
            if (userName == createMsgDto.RecipientUsername.ToLower())
                return ServiceResult.Fail<MessageDto>(msg: "You cannot send messages to yourself.");

            var sender = await _userRepository.GetUserByUsernameAsync(userName);
            var recipient = await _userRepository.GetUserByUsernameAsync(createMsgDto.RecipientUsername);
            if (recipient == null) return ServiceResult.Fail<MessageDto>(msg: "Recipient not found");

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMsgDto.Content
            };

            _msgRepository.AddMessage(message);


            if (await _msgRepository.SaveAllAsync()) return ServiceResult.Success(_mapper.Map<MessageDto>(message));
            
            return ServiceResult.Fail<MessageDto>(msg: "Error in Create Message.");
        }

        public async Task<ServiceResult> DeleteMessage(string currUserName, int id)
        {
            var message = await _msgRepository.GetMessage(id);
            if (message.SenderUsername != currUserName && message.RecipientUsername != currUserName)
            {
                return ServiceResult.Fail(msg: "Unauthorized");
            }

            if (message.SenderUsername == currUserName) message.SenderDeleted = true;
            if (message.RecipientUsername == currUserName) message.RecipientDeleted = true;
            if (message.SenderDeleted && message.RecipientDeleted)
            {
                _msgRepository.DeleteMessage(message);
            }
            if (await _msgRepository.SaveAllAsync()) return ServiceResult.Success();

            return ServiceResult.Fail(msg: "Problem deleting the message");
        }

        public async Task<PageList<MessageDto>> GetMessagesForUserPageList(MessageParams messageParams)
        {
            var messages = await _msgRepository.GetMessages();
            messages = messageParams.Container switch
            {
                "Inbox" => messages.Where(u => u.RecipientUsername == messageParams.Username && !u.RecipientDeleted).ToList(),
                "Outbox" => messages.Where(u => u.SenderUsername == messageParams.Username && !u.SenderDeleted).ToList(),
                _ => messages.Where(u => u.RecipientUsername == messageParams.Username && !u.RecipientDeleted && u.DateRead == null).ToList()
            };
            var messagesDto = _mapper.Map<IEnumerable<MessageDto>>(messages);
            var pageList =  PageList<MessageDto>.CreateAsync(messagesDto, messageParams.PageNumber, messageParams.PageSize);
            return pageList;
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThreads(string currUserName, string userName)
        {
            var messages = await _msgRepository.GetMessageThread(currUserName, userName);
            return _mapper.Map<IEnumerable<MessageDto>>(messages);
        }
        public async Task<Connection> GetConnection(string connId)
        {
            return await _groupRepository.GetConnection(connId);
        }
    }
}
