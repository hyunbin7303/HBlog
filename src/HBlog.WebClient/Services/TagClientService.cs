using HBlog.Contract.Common;
using HBlog.Contract.DTOs;
using System.Net.Http.Json;

namespace HBlog.WebClient.Services
{
    public interface ITagService
    {
        Task<IEnumerable<TagDto>> GetTagsByPostId(int postId);
        Task<IEnumerable<TagDto>> GetTags();
    }
    public class TagClientService : ITagService
    {
        private HttpClient _httpClient;
        public TagClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<TagDto>> GetTags()
        {
            IEnumerable<TagDto>? result = await _httpClient.GetFromJsonAsync<IEnumerable<TagDto>>($"tags");
            return result!;
        }

        public async Task<IEnumerable<TagDto>> GetTagsByPostId(int postId)
        {
            IEnumerable<TagDto>? result = await _httpClient.GetFromJsonAsync<IEnumerable<TagDto>>($"posts/{postId}/tags");
            return result!;
        }
    }
}
