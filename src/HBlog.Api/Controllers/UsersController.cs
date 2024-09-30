using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HBlog.Infrastructure.Extensions;
using HBlog.Domain.Params;
using HBlog.Domain.Common;
using HBlog.Application.Services;
using HBlog.Contract.DTOs;

namespace HBlog.Api.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery]UserParams userParams)
        {
            userParams.CurrentUsername = User.GetUsername();
            var users = await _userService.GetMembersAsync(userParams);
            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));
            return Ok(users);
        }

        [HttpGet("users/{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            var user = await _userService.GetMembersByUsernameAsync(username);
            if (user.Value is null)
                return NotFound($"Input user: {username} cannot find.");

            return Ok(user);
        }
        
        [HttpPut("users")]
        public async Task<IActionResult> Update(UserUpdateDto userUpdateDto)
        {
            if (userUpdateDto is null)
                return BadRequest("Member Update Properties are Empty.");

            var user = await _userService.GetMembersByUsernameAsync(User.GetUsername());
            if (user.Value is null) return NotFound();

            var result = await _userService.UpdateMemberAsync(userUpdateDto);

            return BadRequest("Failed to update user");
        }
    }
}
