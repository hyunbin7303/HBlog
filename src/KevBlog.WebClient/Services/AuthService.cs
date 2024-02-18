using Blazored.LocalStorage;
using KevBlog.Contract.DTOs;
using KevBlog.WebClient.Providers;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace KevBlog.WebClient.Services
{
    public interface IAuthService
    {
        Task<UserDto> AuthenAsync(LoginDto loginDto);
    }
    public class AuthService : IAuthService
    {
        private HttpClient _httpClient;
        private ILocalStorageService _localStorageService;
        private AuthenticationStateProvider _authenStateProvider;
        public AuthService(HttpClient httpClient, ILocalStorageService localStorageService, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
            _authenStateProvider = authenticationStateProvider;
        }

        public async Task<UserDto> AuthenAsync(LoginDto loginDto)
        {
            var result = await _httpClient.PostAsJsonAsync($"Account/login", loginDto);
            var obj = await result.Content.ReadFromJsonAsync<UserDto>();

            await _localStorageService.SetItemAsync("accessToken", obj!.Token);

            await ((ApiAuthStateProvider)_authenStateProvider).LoggedIn();

            return obj;
        }
    }
}
