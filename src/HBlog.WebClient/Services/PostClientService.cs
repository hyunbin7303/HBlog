using Blazored.LocalStorage;
using HBlog.Contract.Common;
using HBlog.Contract.DTOs;
using System;
using System.Net.Http.Json;

namespace HBlog.WebClient.Services
{
    public interface IPostService
    {
        public Task<IEnumerable<PostDisplayDto>> GetPostDisplays(int limit = 5, int offset = 0);
        public Task<IEnumerable<PostDisplayDto>> GetPostDisplayByCategoryId(int categoryId, List<TagDto>? tags = null);
        public Task<PostDisplayDetailsDto> GetPostDetails(int id);
        public Task<bool> CreatePost(PostCreateDto postCreateDto);
        public Task<bool> UpdatePost(PostUpdateDto postUpdateDto);
        public Task<bool> DeletePost(int id);
        public Task<bool> AddTagInPost(int postId, int tagId);
    }
    public class PostClientService : IPostService
    {
        private HttpClient _httpClient;
        private IAuthService _authService;
        public PostClientService(HttpClient httpClient, IAuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
        }
        public async Task<bool> CreatePost(PostCreateDto postCreateDto)
        {
            try
            {
                await _authService.GetBearerToken();
                var result = await _httpClient.PostAsJsonAsync($"Posts", postCreateDto);
                return result.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return false;
            }
        }
        public async Task<bool> UpdatePost(PostUpdateDto postUpdateDto)
        {
            try
            {
                await _authService.GetBearerToken();
                var result = await _httpClient.PutAsJsonAsync($"Posts", postUpdateDto);
                return result.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return false;
            }
        }
        public async Task<bool> DeletePost(int id)
        {
            await _authService.GetBearerToken();
            var result = await _httpClient.DeleteAsync($"Posts/{id}");
            return result.IsSuccessStatusCode;
        }
        public async Task<PostDisplayDetailsDto> GetPostDetails(int id) =>
            await _httpClient.GetFromJsonAsync<PostDisplayDetailsDto>($"Posts/{id}");


        public async Task<IEnumerable<PostDisplayDto>> GetPostDisplays(int limit = 10, int offset = 0)
        {
            var query = new Dictionary<string, string>
            {
                { "limit",  limit.ToString() },
                { "offset", offset.ToString() }
            };
            var url = BuildUrlWithQueryStringUsingUriBuilder($"{_httpClient.BaseAddress}posts", query);

            var result = await _httpClient.GetFromJsonAsync<ApiResponse<IEnumerable<PostDisplayDto>>>(url);
            return result.Data!;
        }

        public async Task<IEnumerable<PostDisplayDto>> GetPostDisplayByCategoryId(int categoryId, List<TagDto>? tags = null)
        {
            var result = await _httpClient.GetFromJsonAsync<IEnumerable<PostDisplayDto>>($"categories/{categoryId}/posts");
            return result!;
        }
        public string BuildUrlWithQueryStringUsingUriBuilder(string basePath, Dictionary<string, string> queryParams)
        {
            var uriBuilder = new UriBuilder(basePath)
            {
                Query = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={kvp.Value}"))
            };
            return uriBuilder.Uri.AbsoluteUri;
        }

        public async Task<bool> AddTagInPost(int postId, int tagId)
        {
            await _authService.GetBearerToken();
            var result = await _httpClient.PutAsJsonAsync($"posts/{postId}/AddTag", tagId);
            return result.IsSuccessStatusCode;
        }
    }
}
