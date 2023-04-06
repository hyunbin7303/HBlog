using KevBlog.Domain.Entities;
namespace KevBlog.Infrastructure.Authentications
{
    public interface ITokenService
    {
        string CreateToken(User user);
        
    }
}