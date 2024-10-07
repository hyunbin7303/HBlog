using HBlog.Contract.DTOs;
using Microsoft.AspNetCore.Identity;
using System.Net.Http.Json;
using System.Text.Json;
namespace HBlog.WebClient.Services
{
    public class UserClientService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UserClientService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<(bool, IEnumerable<IdentityError>?)> RegisterNewUser(RegisterDto registerDto)
        {
            var client = _httpClientFactory.CreateClient("Annoy");
            var result = await client.PostAsJsonAsync($"Account/register", registerDto);
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

        public async ValueTask<MemberDto> GetUserDtoByUsername(string username)
        {
            var client = _httpClientFactory.CreateClient("Auth");
            var result = await client.GetFromJsonAsync<HttpResponseMessage>($"users/{username}");
            if (result!.IsSuccessStatusCode)
            {
               return await result.Content.ReadFromJsonAsync<MemberDto>();

            }
            return new MemberDto();
               
        }

        public async ValueTask<IEnumerable<MemberDto>> GetUsers()
        {
            var client = _httpClientFactory.CreateClient("Auth");
            var result = await client.GetFromJsonAsync<HttpResponseMessage>($"users");
            if (result!.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<IEnumerable<MemberDto>>();

            }
            return default;

        }
    }
}
