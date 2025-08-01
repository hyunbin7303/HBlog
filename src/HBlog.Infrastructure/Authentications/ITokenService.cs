using HBlog.Domain.Entities;
namespace HBlog.Infrastructure.Authentications
{
    public interface ITokenService
    {
        Task<string> CreateToken(User user);
        public string CreateRefreshToken();
    }
}