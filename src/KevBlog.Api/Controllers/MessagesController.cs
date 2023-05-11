using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using KevBlog.Application.DTOs;
using KevBlog.Application.Services;
using KevBlog.Domain.Common;
using KevBlog.Domain.Entities;
using KevBlog.Domain.Params;
using KevBlog.Domain.Repositories;
using KevBlog.Infrastructure.Extensions;
using KevBlog.Infrastructure.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KevBlog.Api.Controllers
{
    public class MessagesController : BaseApiController
    {
        private readonly IMessageRepository _msgRepository;
        private readonly IMessageService _messageService;
        public MessagesController(IMessageRepository messageRepository, IMessageService messageService)
        {
            _messageService= messageService;
            _msgRepository = messageRepository;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(MessageCreateDto createMsgDto)
        {
            var username = User.GetUsername();
            if(username == createMsgDto.RecipientUsername.ToLower()){
                return BadRequest("You cannot send messages to yourself.");
            }
            var msgDto = await _messageService.CreateMessage(username, createMsgDto);
            if(msgDto is null)
                return BadRequest("Failed to send message.");
            return Ok(msgDto);
        }

        [HttpGet]
        public async Task<ActionResult<PageList<MessageDto>>> GetMessagesForUser([FromQuery]MessageParams messageParams)
        {
            messageParams.Username = User.GetUsername();
            var messages = await _messageService.GetMessagesForUserPageList(messageParams);
            Response.AddPaginationHeader(new PaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages));
             
            return messages;
        }

        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string username)
        {
            var msgs = await _messageService.GetMessageThreads(User.GetUsername(), username);
            return Ok(msgs);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id) {
            var currUsername = User.GetUsername();
            var message = await _msgRepository.GetMessage(id);

            if(message.SenderUsername!= currUsername && message.RecipientUsername != currUsername) {
                return Unauthorized();
            }

            if(message.SenderUsername == currUsername) message.SenderDeleted = true;
            if(message.RecipientUsername == currUsername) message.RecipientDeleted = true;
            if(message.SenderDeleted && message.RecipientDeleted) {
                _msgRepository.DeleteMessage(message);
            }
            if(await _msgRepository.SaveAllAsync()) return Ok();

            return BadRequest("Problem deleting the message");
        }

    }
}