using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using KevBlog.Infrastructure.Extensions;
using KevBlog.Domain.Params;
using KevBlog.Domain.Common;
using KevBlog.Application.Services;
using KevBlog.Contract.DTOs;

namespace KevBlog.Api.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery]UserParams userParams)
        {
            userParams.CurrentUsername = User.GetUsername();
            var users = await _userService.GetMembersAsync(userParams);
            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));
            return Ok(users);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            var user = await _userService.GetMembersByUsernameAsync(username);
            if (user.Value is null)
                return NotFound($"Input user: {username} cannot find.");

            return Ok(user);
        }
        
        [HttpPut]
        public async Task<IActionResult> Update(MemberUpdateDto memberUpdateDto)
        {
            if (memberUpdateDto is null)
                return BadRequest("Member Update Properties are Empty.");

            var user = await _userService.GetMembersByUsernameAsync(User.GetUsername());
            if (user.Value is null) return NotFound();

            var result = await _userService.UpdateMemberAsync(memberUpdateDto);

            return BadRequest("Failed to update user");
        }
    }
}
