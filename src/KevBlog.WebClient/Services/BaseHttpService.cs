using Blazored.LocalStorage;
using System.Net.Http.Headers;

namespace KevBlog.WebClient.Services
{
    public interface IHttpService
    {
        Task<T> Get<T>(string uri);
        Task<T> Post<T>(string uri, object value);
    }
    public class BaseHttpService : IHttpService
    {
        public readonly HttpClient _httpClient;
        public readonly ILocalStorageService _localStorageService;
        public BaseHttpService(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:5001/api/");
            _localStorageService = localStorageService;
        }

        public Task<T> Get<T>(string uri)
        {
            throw new NotImplementedException();
        }

        public Task<T> Post<T>(string uri, object value)
        {
            throw new NotImplementedException();
        }

        protected async Task GetBearerToken()
        {
            var token= await _localStorageService.GetItemAsync<string>("accessToken");
            if (token != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
    }
}
