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
        private readonly IMessageService _messageService;
        public MessagesController(IMessageService messageService)
        {
            _messageService= messageService;
        }

        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string username) => Ok(await _messageService.GetMessageThreads(User.GetUsername(), username));

        [HttpGet]
        public async Task<ActionResult<PageList<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
        {
            messageParams.Username = User.GetUsername();
            var messages = await _messageService.GetMessagesForUserPageList(messageParams);
            Response.AddPaginationHeader(new PaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages));

            return messages;
        }
        [HttpPost] 
        public async Task<ActionResult<MessageDto>> CreateMessage(MessageCreateDto createMsgDto)
        {
            if (createMsgDto is null)
                throw new ArgumentNullException(nameof(createMsgDto));

            var msgResult = await _messageService.CreateMessage(User.GetUsername(), createMsgDto);
            if (!msgResult.IsSuccess)
                return BadRequest(msgResult.Message);

            return Ok(msgResult);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id) {
            var currUsername = User.GetUsername();
            var result = await _messageService.DeleteMessage(currUsername, id);
            if(!result.IsSuccess) {
                if(result.Message == "Unauthorized")
                    return Unauthorized();

                return BadRequest(result.Message);
            }
            return Ok();
        }

    }
}