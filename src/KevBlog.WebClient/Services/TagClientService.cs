using KevBlog.Contract.DTOs;
using System.Net.Http.Json;

namespace KevBlog.WebClient.Services
{
    public interface ITagService
    {
        Task<IEnumerable<TagDto>> GetTagsByPostId(int postId);
    }
    public class TagClientService : ITagService
    {
        private HttpClient _httpClient;
        public TagClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<TagDto>> GetTagsByPostId(int postId)
        {
            IEnumerable<TagDto>? result = await _httpClient.GetFromJsonAsync<IEnumerable<TagDto>>($"posts/{postId}/tags");
            return result!;
        }
    }
}
