using KevBlog.Domain.Entities;
namespace KevBlog.Infrastructure.Authentications
{
    public interface ITokenService
    {
        Task<string> CreateToken(User user);
        
    }
}