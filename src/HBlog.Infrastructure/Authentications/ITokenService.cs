using HBlog.Domain.Entities;
using System.Security.Claims;
namespace HBlog.Infrastructure.Authentications
{
    public interface ITokenService
    {
        Task<string> CreateToken(User user);
        public string CreateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

    }
}