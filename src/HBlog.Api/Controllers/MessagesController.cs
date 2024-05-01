using HBlog.Application.Services;
using HBlog.Contract.DTOs;
using HBlog.Domain.Common;
using HBlog.Domain.Params;
using HBlog.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace HBlog.Api.Controllers
{
    public class MessagesController : BaseApiController
    {
        private readonly IMessageService _messageService;
        public MessagesController(IMessageService messageService)
        {
            _messageService= messageService;
        }

        [HttpGet("Messages/thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string username) => Ok(await _messageService.GetMessageThreads(User.GetUsername(), username));

        [HttpGet("messages")]
        public async Task<ActionResult<PageList<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
        {
            messageParams.Username = User.GetUsername();
            var messages = await _messageService.GetMessagesForUserPageList(messageParams);
            Response.AddPaginationHeader(new PaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages));

            return messages;
        }
        
        [HttpPost("messages")] 
        public async Task<ActionResult<MessageDto>> CreateMessage(MessageCreateDto createMsgDto)
        {
            if (createMsgDto is null)
                throw new ArgumentNullException(nameof(createMsgDto));

            var msgResult = await _messageService.CreateMessage(User.GetUsername(), createMsgDto);
            if (!msgResult.IsSuccess)
                return BadRequest(msgResult.Message);

            return Ok(msgResult);
        }

        [HttpDelete("messages/{id}")]
        public async Task<ActionResult> DeleteMessage(int id) {
            var result = await _messageService.DeleteMessage(User.GetUsername(), id);
            if(!result.IsSuccess) {
                if(result.Message == "Unauthorized")
                    return Unauthorized();

                return BadRequest(result.Message);
            }
            return Ok();
        }

    }
}