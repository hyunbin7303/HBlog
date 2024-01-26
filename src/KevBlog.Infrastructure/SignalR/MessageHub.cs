using AutoMapper;
using KevBlog.Application.Services;
using KevBlog.Contract.DTOs;
using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using KevBlog.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace KevBlog.Infrastructure.SignalR
{
    [Authorize]
    public class MessageHub : Hub
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMessageService _messageService;
        private readonly IUserRepository _userRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IMapper _mapper;
        public MessageHub(IMessageService messageService, IMessageRepository messageRepository, IUserRepository userRepository, IGroupRepository groupRepository, IMapper mapper)
        {
            _messageService = messageService;
            _messageRepository = messageRepository;
            _userRepository = userRepository;
            _groupRepository = groupRepository;
            _mapper = mapper;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var otherUser = httpContext.Request.Query["user"];
            var groupName = GetGroupName(Context.User.GetUsername(), otherUser);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            var messages = await _messageService.GetMessageThreads(Context.User.GetUsername(), otherUser);
            await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
        public async Task SendMessage(MessageCreateDto createMsgDto)
        {
            var username = Context.User.GetUsername();
            if(username == createMsgDto.RecipientUsername.ToLower())
                throw new HubException("You cannot send messages to yourself.");

            var sender = await _userRepository.GetUserByUsernameAsync(username);
            var recipient = await _userRepository.GetUserByUsernameAsync(createMsgDto.RecipientUsername);
            if (recipient == null) throw new HubException("Recipient not found");

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMsgDto.Content
            };

            _messageRepository.AddMessage(message);
            if (await _messageRepository.SaveAllAsync())
            {
                var group = GetGroupName(sender.UserName, recipient.UserName);
                await Clients.Group(group).SendAsync("NewMessage", _mapper.Map<MessageDto>(message));
            }
        }

        private string GetGroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }

        private async Task<bool> AddToGroup(string groupName)
        {
            var group = await _groupRepository.GetMsgGroup(groupName);
            var connection = new Connection(Context.ConnectionId, Context.User.GetUsername());
            if(group is null)
            {
                group = new Group(groupName);
                _groupRepository.AddGroup(group);
            }
            group.Connections.Add(connection);
            return await _messageRepository.SaveAllAsync(); 
        }

        private async Task RemoveFromMessageGroup()
        {

        }
    }
}
