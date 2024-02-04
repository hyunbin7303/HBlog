using KevBlog.Contract.DTOs;

namespace KevBlog.WebClient.Services
{
    public interface IUserService
    {
        Task<bool> RegisterNewUser(RegisterDto registerDto);
        Task<bool> Login(LoginDto loginDto);
    }
    public class UserClientService : IUserService
    {
        public Task<bool> Login(LoginDto loginDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RegisterNewUser(RegisterDto registerDto)
        {
            throw new NotImplementedException();
        }
    }
}
