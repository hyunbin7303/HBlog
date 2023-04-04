using Microsoft.AspNetCore.Mvc;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using API.Interfaces;
using AutoMapper;
using API.DTOs;
using System.Security.Claims;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            this._mapper = mapper;
            this._userRepository = userRepository;
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await _userRepository.GetMembersAsync();
            return Ok(users);
        }


        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            return await _userRepository.GetMemberAsync(username);
        }
        [HttpPut]
        public async Task<IActionResult> Update(MemberUpdateDto memberUpdateDto)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; 
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if(user == null) return NotFound();

            _mapper.Map(memberUpdateDto, user);
            if(await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update user");
        }

        // POST: api/AppUsers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostAppUser(User appUser)
        {
            // _context.Users.Add(appUser);
            // await _context.SaveChangesAsync();

            // return CreatedAtAction("GetAppUser", new { id = appUser.Id }, appUser);
            return Ok();
        }

        // DELETE: api/AppUsers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppUser(int id)
        {
            // var appUser = await _context.Users.FindAsync(id);
            // if (appUser == null)
            // {
            //     return NotFound();
            // }

            // _context.Users.Remove(appUser);
            // await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AppUserExists(int id)
        {
            // return _context.Users.Any(e => e.Id == id);
            return true;
        }
    }
}
