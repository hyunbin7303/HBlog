using Blazored.LocalStorage;
using KevBlog.Contract.DTOs;
using System.Net.Http.Json;

namespace KevBlog.WebClient.Services
{
    public interface IPostService
    {
        public Task<IEnumerable<PostDisplayDto>> GetPostDisplays();
        public Task<PostDisplayDetailsDto> GetPostDetails(int id);
        public Task<bool> CreatePost(PostCreateDto postCreateDto);
    }
    public class PostClientService : BaseHttpService, IPostService
    {
        public PostClientService(HttpClient httpClient, ILocalStorageService localStorage) : base(httpClient, localStorage)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:5001/api/");
        }

        public async Task<bool> CreatePost(PostCreateDto postCreateDto)
        {
            try
            {
                await GetBearerToken();
                var result = await _httpClient.PostAsJsonAsync($"Posts", postCreateDto);
                return result.IsSuccessStatusCode;
            }
            catch(Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
            return false;
        }

        public async Task<PostDisplayDetailsDto> GetPostDetails(int id) =>
            await _httpClient.GetFromJsonAsync<PostDisplayDetailsDto>($"Posts/{id}");

        public async Task<IEnumerable<PostDisplayDto>> GetPostDisplays() =>
             await _httpClient.GetFromJsonAsync<IEnumerable<PostDisplayDto>>($"Posts");
    }
}
