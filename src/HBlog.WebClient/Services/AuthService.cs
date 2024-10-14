using Blazored.LocalStorage;
using HBlog.Contract.Common;
using HBlog.Contract.DTOs;
using HBlog.WebClient.Providers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace HBlog.WebClient.Services
{
    public interface IAuthService
    {
        Task<AccountDto> AuthenAsync(LoginDto loginDto);
        Task InjectToken();
        Task Logout(); 
    }
    public class AuthService(
        HttpClient httpClient,
        ILogger<PostClientService> logger,
        ILocalStorageService localStorageService,
        AuthenticationStateProvider authenticationStateProvider)
        : BaseService(httpClient, logger), IAuthService
    {
        public async Task<AccountDto> AuthenAsync(LoginDto loginDto)
        {
            var result = await _httpClient.PostAsJsonAsync($"Account/login", loginDto);
            var obj = await result.Content.ReadFromJsonAsync<AccountDto>();

            await localStorageService.SetItemAsync(Constants.AccessToken, obj!.Token);

            await ((ApiAuthStateProvider)authenticationStateProvider).LoggedIn();

            return obj;
        }
        public async Task InjectToken()
        {
            var token = await localStorageService.GetItemAsync<string>(Constants.AccessToken);
            if (token != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
        public async Task Logout()
        {
            await ((ApiAuthStateProvider)authenticationStateProvider).LoggedOut();
        }
    }
}
