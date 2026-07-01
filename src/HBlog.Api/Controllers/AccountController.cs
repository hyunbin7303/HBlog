using AutoMapper;
using HBlog.Contract.DTOs;
using HBlog.Contract.DTOs.OAuth;
using HBlog.Domain.Entities;
using HBlog.Infrastructure.Authentications;
using HBlog.Infrastructure.Authentications.OAuth;
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
        private readonly IExternalIdentityProviderRegistry _providerRegistry;
        private readonly IOAuthTicketService _ticketService;
        public ITokenService _tokenService { get; }
        public IMapper _mapper { get; }
        public AccountController(
            UserManager<User> userManager,
            ITokenService tokenService,
            IMapper mapper,
            IExternalIdentityProviderRegistry providerRegistry,
            IOAuthTicketService ticketService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
            _providerRegistry = providerRegistry;
            _ticketService = ticketService;
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
            if (user == null) return Unauthorized(new Microsoft.AspNetCore.Mvc.ProblemDetails { Status = (int)HttpStatusCode.Unauthorized, Title = "Unauthorized", Detail = "Invalid username" });

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result)
                return Unauthorized(new Microsoft.AspNetCore.Mvc.ProblemDetails { Status = (int)HttpStatusCode.Unauthorized, Title = "Unauthorized", Detail = "Invalid password" });

            user.RefreshToken = _tokenService.CreateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            return new AccountDto
            {
                Username = user.UserName!,
                Email = user.Email,
                Token = await _tokenService.CreateToken(user),
                RefreshToken = user.RefreshToken,
            };
        }

        [HttpPost("account/refresh")]
        public async Task<ActionResult<AccountDto>> RefreshToken(RefreshTokenDto refreshTokenDto)
        {
            if(refreshTokenDto is null) return BadRequest(new Microsoft.AspNetCore.Mvc.ProblemDetails { Status = (int)HttpStatusCode.BadRequest, Title = "Bad Request", Detail = "Refresh token cannot be null" });

            var principal = _tokenService.GetPrincipalFromExpiredToken(refreshTokenDto?.Token);
            var username = principal.Identity?.Name;

            var user = await _userManager.FindByNameAsync(username);
            if (user is null || user.RefreshToken != refreshTokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
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


        [HttpPost("account/oauth/{provider}")]
        public async Task<ActionResult<object>> OAuthLogin(string provider, OAuthLoginDto dto, CancellationToken ct)
        {
            var handler = _providerRegistry.Find(provider);
            if (handler is null) return BadRequest($"Unsupported OAuth provider: {provider}");

            var identity = await handler.VerifyAsync(dto, ct);
            if (identity is null) return Unauthorized401($"Invalid {handler.Name} ID token");
            return await ResolveOrTicketAsync(identity);
        }

        [HttpPost("account/oauth/complete")]
        public async Task<ActionResult<AccountDto>> OAuthComplete(OAuthCompleteDto dto)
        {
            var identity = _ticketService.Validate(dto.OAuthTicket);
            if (identity is null) return Unauthorized401("Invalid or expired oauthTicket");

            var existingLink = await _userManager.FindByLoginAsync(identity.Provider, identity.Subject);
            if (existingLink is not null) return await IssueTokensAsync(existingLink);

            if (await UserExists(dto.UserName)) return BadRequest("Username is taken");
            if (await EmailExists(dto.Email)) return BadRequest("Email is taken");

            var user = new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                FirstName = string.IsNullOrWhiteSpace(dto.FirstName) ? (identity.GivenName ?? string.Empty) : dto.FirstName,
                LastName = string.IsNullOrWhiteSpace(dto.LastName) ? (identity.FamilyName ?? string.Empty) : dto.LastName,
                KnownAs = dto.UserName,
                EmailConfirmed = identity.EmailVerified,
            };

            var createResult = await _userManager.CreateAsync(user);
            if (!createResult.Succeeded) return BadRequest(createResult.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "Member");
            if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);

            var loginResult = await _userManager.AddLoginAsync(user,
                new UserLoginInfo(identity.Provider, identity.Subject, identity.Provider));
            if (!loginResult.Succeeded) return BadRequest(loginResult.Errors);

            return await IssueTokensAsync(user);
        }

        private async Task<ActionResult<object>> ResolveOrTicketAsync(ExternalIdentity identity)
        {
            var user = await _userManager.FindByLoginAsync(identity.Provider, identity.Subject);
            if (user is not null)
            {
                var account = await IssueTokensAsync(user);
                return account.Result ?? (ActionResult<object>)account.Value!;
            }

            return Ok(new OAuthTicketResponseDto
            {
                Status = "needs_profile",
                OAuthTicket = _ticketService.Issue(identity),
            });
        }

        private async Task<ActionResult<AccountDto>> IssueTokensAsync(User user)
        {
            user.RefreshToken = _tokenService.CreateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);
            return new AccountDto
            {
                Username = user.UserName!,
                Email = user.Email,
                Token = await _tokenService.CreateToken(user),
                RefreshToken = user.RefreshToken,
            };
        }

        private UnauthorizedObjectResult Unauthorized401(string detail) =>
            Unauthorized(new Microsoft.AspNetCore.Mvc.ProblemDetails
            {
                Status = (int)HttpStatusCode.Unauthorized,
                Title = "Unauthorized",
                Detail = detail,
            });

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
