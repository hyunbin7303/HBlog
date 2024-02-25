using Blazored.LocalStorage;
using KevBlog.Contract.DTOs;
using System.Net.Http.Json;
namespace KevBlog.WebClient.Services
{
    public class UserClientService : BaseHttpService
    {
        public UserClientService(HttpClient httpClient, ILocalStorageService localStorageService) 
            : base(httpClient, localStorageService){ }
        public async Task<bool> RegisterNewUser(RegisterDto registerDto)
        {
            var result = await _httpClient.PostAsJsonAsync($"Account/register", registerDto);
            return result.IsSuccessStatusCode;
        }
    }
}
