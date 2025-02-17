using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HBlog.Domain.Entities;
using HBlog.Infrastructure.Authentications;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using HBlog.Contract.DTOs;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace HBlog.Api.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<User> _userManager;
        public ITokenService _tokenService { get; }
        public IMapper _mapper { get; }
        public AccountController(UserManager<User> userManager, ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("account/register")]
        public async Task<ActionResult<AccountDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.UserName)) return BadRequest("Username is taken");
            if (await EmailExists(registerDto.Email)) return BadRequest("Email is taken");

            var user = _mapper.Map<User>(registerDto);

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "Member");
            if(!roleResult.Succeeded) return BadRequest(roleResult.Errors);

            // new AccountDto
            // {
            //     Username = user.UserName,
            //     Token = await _tokenService.CreateToken(user),
            //     KnownAs = user.KnownAs
            // };
            return Created();
        }

        [HttpPost("account/login")]
        public async Task<ActionResult<AccountDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.Users.Include(p=> p.Photos).FirstOrDefaultAsync(x => x.UserName == loginDto.UserName);
            if (user == null) return Unauthorized("Invalid Username");

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result) return Unauthorized("Invalid password");

            return new AccountDto
            {
                Username = user.UserName,
                Email = user.Email,
                Token = await _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }

        [HttpPut("account/{id}")]
        public async Task<IActionResult> PutAppUser(int id, User appUser)
        {
            if (id != appUser.Id)
            {
                return BadRequest();
            }

            //_context.Entry(appUser).State = EntityState.Modified;

            try
            {
                //await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppUserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost("account")]
        public ActionResult<User> PostAppUser(User appUser)
        {
            //var result = _userManager.Users.Add(appUser);
            //await _context.SaveChangesAsync();
            return CreatedAtAction("GetAppUser", new { id = appUser.Id }, appUser);
        }

        [HttpDelete("account/{id}")]
        public IActionResult DeleteAppUser(int id)
        {
            //var appUser = await _userManager.Users.(id);
            //if (appUser == null)
            //{
            //    return NotFound();
            //}
            //_userManager.Users.Remove(appUser);
            return NoContent();
        }
        private bool AppUserExists(int id)
        {
            return _userManager.Users.Any(e => e.Id == id);
        }
        private async Task<bool> UserExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
        private async Task<bool> EmailExists(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email == email.ToLower());
        }
    }
}
