using AutoMapper;
using AutoMapper.QueryableExtensions;
using KevBlog.Application.DTOs;
using KevBlog.Domain.Common;
using KevBlog.Domain.Entities;
using KevBlog.Domain.Params;
using KevBlog.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KevBlog.Application.Services
{
    public class MessageService : BaseService,  IMessageService
    {
        private readonly IMessageRepository _msgRepository;
        private readonly IUserRepository _userRepository;
        public MessageService(IMapper mapper,IMessageRepository messageRepository,IUserRepository userRepository) : base(mapper)
        {
            _msgRepository = messageRepository;
            _userRepository = userRepository;
        }

        public async Task<MessageDto> CreateMessage(string userName, MessageCreateDto createMsgDto)
        {
            var sender = await _userRepository.GetUserByUsernameAsync(userName);
            var recipient = await _userRepository.GetUserByUsernameAsync(createMsgDto.RecipientUsername);
            if (recipient == null) throw new Exception("Recipient not found");

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMsgDto.Content
            };

            _msgRepository.AddMessage(message);
            if (await _msgRepository.SaveAllAsync()) return _mapper.Map<MessageDto>(message);
            return null;
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
            var messagesDto = _mapper.Map<IEnumerable<MessageDto>>(messages).AsQueryable();
            var pageList = await PageList<MessageDto>.CreateAsync(messagesDto, messageParams.PageNumber, messageParams.PageSize);
            return pageList;
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThreads(string currUserName, string userName)
        {
            var messages = await _msgRepository.GetMessageThread(currUserName, userName);
            return _mapper.Map<IEnumerable<MessageDto>>(messages);
        }
    }
}
