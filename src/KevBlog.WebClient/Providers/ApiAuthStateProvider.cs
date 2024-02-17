using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace KevBlog.WebClient.Providers
{
    public class ApiAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        public ApiAuthStateProvider(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity());
            var saveToken = await _localStorageService.GetItemAsync<string>("accessToken");
            if (saveToken == null)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var tokenContent = _jwtSecurityTokenHandler.ReadJwtToken(saveToken);
            if(tokenContent.ValidTo < DateTime.Now)
            {
                return new AuthenticationState(user);
            }
            
            var claims = tokenContent.Claims;
            user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
            return new AuthenticationState(user);
        }


        public async Task LoggedIn()
        {
            var savedToken = await _localStorageService.GetItemAsync<string>("accessToken");
            var tokenContent = _jwtSecurityTokenHandler.ReadJwtToken(savedToken);
            var claims = tokenContent.Claims;
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
            var authState = Task.FromResult(new AuthenticationState(user));
            NotifyAuthenticationStateChanged(authState);
        }

        public void LoggedOut()
        {
            var nobody = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(nobody));
            NotifyAuthenticationStateChanged(authState);
        }
    }
}
