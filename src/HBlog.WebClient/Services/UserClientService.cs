using HBlog.Contract.DTOs;
using Microsoft.AspNetCore.Identity;
using System.Net.Http.Json;
using System.Text.Json;
namespace HBlog.WebClient.Services
{
    public class UserClientService(HttpClient httpClient, ILogger<UserClientService> logger, IAuthService authService)
        : BaseService(httpClient, logger)
    {
        private readonly IAuthService _authService = authService;

        public async Task<(bool, IEnumerable<IdentityError>?)> RegisterNewUser(RegisterDto registerDto)
        {
            var result = await _httpClient.PostAsJsonAsync($"Account/register", registerDto);
            if (result.IsSuccessStatusCode)
                return (true, null);
            
            var responseJson = await result.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var responseData = JsonSerializer.Deserialize<IEnumerable<IdentityError>>(responseJson, options);
            return (false, responseData);
        }

        public async ValueTask<UserDto> GetUserDtoByUsername(string username)
        {
            await _authService.InjectToken();
            var result = await _httpClient.GetFromJsonAsync<HttpResponseMessage>($"users/{username}");
            if (result!.IsSuccessStatusCode)
            {
               return await result.Content.ReadFromJsonAsync<UserDto>();
            }
            return new UserDto();
               
        }

        public async ValueTask<IEnumerable<UserDto>> GetUsers()
        {
            var result = await _httpClient.GetFromJsonAsync<HttpResponseMessage>($"users");
            if (result!.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<IEnumerable<UserDto>>();

            }
            return default;

        }
    }
}
