using AutoMapper;
using HBlog.Contract.DTOs;
using HBlog.Domain.Entities;
using HBlog.Infrastructure.Authentications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

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

            return Created();
        }

        [HttpPost("account/login")]
        public async Task<ActionResult<AccountDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user == null) return Unauthorized(new ProblemDetails { Status = (int)HttpStatusCode.Unauthorized, Title = "Unauthorized", Detail = "Invalid username" });

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result)
                return Unauthorized(new ProblemDetails { Status = (int)HttpStatusCode.Unauthorized, Title = "Unauthorized", Detail = "Invalid password" });

            return new AccountDto
            {
                Username = user.UserName!,
                Email = user.Email,
                Token = await _tokenService.CreateToken(user),
                RefreshToken = _tokenService.CreateRefreshToken(),
            };
        }

        [HttpPost("account/refresh")]
        public async Task<ActionResult<AccountDto>> RefreshToken(RefreshTokenDto refreshTokenDto)
        {
            if(refreshTokenDto is null) return BadRequest(new ProblemDetails { Status = (int)HttpStatusCode.BadRequest, Title = "Bad Request", Detail = "Refresh token cannot be null" });

            var principal = _tokenService.GetPrincipalFromExpiredToken(refreshTokenDto.AccessToken);
            var username = principal.Identity?.Name;

            var user = await _userManager.FindByNameAsync(username);
            if (user is null || user.RefreshToken != refreshTokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return BadRequest("Invalid client request");

            user.RefreshToken = _tokenService.CreateRefreshToken();
            await _userManager.UpdateAsync(user);
            return new AccountDto
            {
                Username = user.UserName!,
                Email = user.Email,
                Token = await _tokenService.CreateToken(user),
                RefreshToken = user.RefreshToken!,
            };
        }

        [HttpPost, Authorize]
        [Route("account/revoke")]
        public async Task<IActionResult> Revoke()
        {
            var username = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return BadRequest("User not exist");
            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);
            return NoContent();
        }


        [HttpPut("account/{id}" )]
        public async Task<IActionResult> PutAppUser(Guid id, User appUser)
        {
            if (id != appUser.Id)
            {
                return BadRequest();
            }
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

        private bool AppUserExists(Guid id)
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
