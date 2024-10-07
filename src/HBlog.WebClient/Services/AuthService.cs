using Blazored.LocalStorage;
using HBlog.Contract.Common;
using HBlog.Contract.DTOs;
using HBlog.WebClient.Providers;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace HBlog.WebClient.Services
{
    public interface IAuthService
    {
        Task<UserDto> AuthenAsync(LoginDto loginDto);
        Task<string> GetBearerToken();
        Task Logout(); 
    }
    public class AuthService : IAuthService
    {
        private IHttpClientFactory _httpClientFactory;
        private ILocalStorageService _localStorageService;
        private AuthenticationStateProvider _authenStateProvider;
        public AuthService(IHttpClientFactory httpClientFactory, ILocalStorageService localStorageService, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClientFactory = httpClientFactory;
            _localStorageService = localStorageService;
            _authenStateProvider = authenticationStateProvider;
        }

        public async Task<UserDto> AuthenAsync(LoginDto loginDto)
        {
            var _httpClient = _httpClientFactory.CreateClient("Annoy");
            var result = await _httpClient.PostAsJsonAsync($"Account/login", loginDto);
            var obj = await result.Content.ReadFromJsonAsync<UserDto>();

            await _localStorageService.SetItemAsync(Constants.AccessToken, obj!.Token);

            await ((ApiAuthStateProvider)_authenStateProvider).LoggedIn();

            return obj;
        }
        public async Task<string> GetBearerToken()
        {
            return await _localStorageService.GetItemAsync<string>(Constants.AccessToken) ?? string.Empty;
        }
        public async Task Logout()
        {
            await ((ApiAuthStateProvider)_authenStateProvider).LoggedOut();
        }
    }
}
