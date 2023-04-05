
using KevBlog.Domain.Entities;

namespace API.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
        
    }
}