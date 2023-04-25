using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using KevBlog.Domain.Entities;
using KevBlog.Application.DTOs;
using KevBlog.Infrastructure.Helpers;
using KevBlog.Infrastructure.Extensions;
using KevBlog.Domain.Repositories;

namespace KevBlog.Api.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery] UserParams userParams)
        {
            var currUser = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            userParams.CurrentUsername = currUser.UserName;

            if(string.IsNullOrEmpty(userParams.Gender)) {
                userParams.Gender = currUser.Gender == "male" ? "female" : "male";
            }

            var users = await GetMembersAsync(userParams);
            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));
            return Ok(users);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            var user = await GetMembersByUsernameAsync(username);
            if (user is null)
                return NotFound($"Input user: {username} cannot find.");

            return Ok(user);
        }
        [HttpPut]
        public async Task<IActionResult> Update(MemberUpdateDto memberUpdateDto)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            if (user == null) return NotFound();

            _mapper.Map(memberUpdateDto, user);
            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update user");
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<User>> Add(User appUser)
        {
            await _userRepository.Add(appUser);
            return CreatedAtAction("GetAppUser", new { id = appUser.Id }, appUser);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppUser(int id)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            if (user == null) return NotFound();

            return NoContent();
        }

        private bool AppUserExists(int id)
        {
            return true;
        }

        private async Task<MemberDto> GetMembersByUsernameAsync(string username)
        {
            User user = await _userRepository.GetUserByUsernameAsync(username);
            return _mapper.Map<MemberDto>(user) ?? null;
        }
        private async Task<PageList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            var userQuery = _userRepository.GetUserQuery();
            userQuery = userQuery.Where(x => x.UserName != userParams.CurrentUsername);
            userQuery = userQuery.Where(x => x.Gender == userParams.Gender);

            var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
            var maxDob = DateTime.Today.AddYears(-userParams.MinAge);
            userQuery = userQuery.Where(x => x.DateOfBirth >= minDob && x.DateOfBirth <= maxDob);

            userQuery = userParams.OrderBy switch 
            {
                "created" => userQuery.OrderByDescending(x => x.Created),
                _ => userQuery.OrderByDescending(x => x.LastActive)
            };

            var memberQuery = _mapper.ProjectTo<MemberDto>(userQuery);
            return await PageList<MemberDto>.CreateAsync(memberQuery, userParams.PageNumber, userParams.PageSize);
        }
    }
}
