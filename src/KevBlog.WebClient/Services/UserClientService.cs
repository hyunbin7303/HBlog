using KevBlog.Contract.DTOs;
using System.Net.Http.Json;

namespace KevBlog.WebClient.Services
{
    public class UserClientService 
    {
        private HttpClient _httpClient;
        public UserClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:5001/api/");
        }

        public async Task<bool> Login(LoginDto loginDto)
        {
            var result = await _httpClient.PostAsJsonAsync($"Account/login", loginDto);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> RegisterNewUser(RegisterDto registerDto)
        {
            var result = await _httpClient.PostAsJsonAsync($"Account/register", registerDto);
            return result.IsSuccessStatusCode;
        }
    }
}
