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
    public class PostClientSerivce : IPostService
    {
        private HttpClient _httpClient;
        public PostClientSerivce(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:5001/api/");
            //_httpClient.DefaultRequestHeaders.Accept.Add(new("application/json"));
        }

        public async Task<bool> CreatePost(PostCreateDto postCreateDto)
        {
            var result = await _httpClient.PostAsJsonAsync($"Posts", postCreateDto);
            return result.IsSuccessStatusCode;
        }

        public async Task<PostDisplayDetailsDto> GetPostDetails(int id) =>
            await _httpClient.GetFromJsonAsync<PostDisplayDetailsDto>($"Posts/{id}");

        public async Task<IEnumerable<PostDisplayDto>> GetPostDisplays() =>
             await _httpClient.GetFromJsonAsync<IEnumerable<PostDisplayDto>>($"Posts");
    }
}
