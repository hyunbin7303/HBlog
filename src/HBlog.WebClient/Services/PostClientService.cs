using Blazored.LocalStorage;
using HBlog.Contract.Common;
using HBlog.Contract.DTOs;
using HBlog.WebClient.Helpers;
using System.Collections.Generic;
using System.Net.Http.Json;

namespace HBlog.WebClient.Services
{
    public interface IPostService
    {
        public Task<IEnumerable<PostDisplayDto>> GetPostDisplays(int limit = 10, int offset = 0);
        public Task<IEnumerable<PostDisplayDto>> GetPostDisplayByFilters(int categoryId, List<TagDto>? tags = null);
        public Task<PostDisplayDetailsDto> GetPostDetails(int id);
        public Task<bool> CreatePost(PostCreateDto postCreateDto);
        public Task<bool> UpdatePost(PostUpdateDto postUpdateDto);
        public Task<bool> DeletePost(int id);   
        public Task<bool> AddTagInPost(int postId, int tagId);
    }
    public class PostClientService : BaseService, IPostService
    {
        public PostClientService(HttpClient httpClient,ILogger<PostClientService> logger) : base(httpClient,logger)
        {
        }
        public async Task<bool> CreatePost(PostCreateDto postCreateDto)
        {
            try
            {
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
            var url = QueryHelper.BuildUrlWithQueryStringUsingUriBuilder($"{_httpClient.BaseAddress}posts", query);

            var result = await _httpClient.GetFromJsonAsync<ApiResponse<IEnumerable<PostDisplayDto>>>(url);
            return result.Data!;
        }

        public async Task<IEnumerable<PostDisplayDto>> GetPostDisplayByFilters(int categoryId, List<TagDto>? tags = null)
        {
            var query = new Dictionary<string, string>
            {
                { "categoryId", categoryId.ToString() },
            };
            
            string url = QueryHelper.BuildUrlWithQueryStringUsingUriBuilder($"{_httpClient.BaseAddress}posts", query);
            if (tags is not null)
                url +=  "&" + QueryHelper.ArrayToQueryString("tagid", tags.Select(x => x.TagId.ToString()).ToArray());

            await Console.Out.WriteLineAsync(url);
            var result = await _httpClient.GetFromJsonAsync<ApiResponse<IEnumerable<PostDisplayDto>>>(url);
            return result.Data!;
        }
        public async Task<bool> AddTagInPost(int postId, int tagId)
        {
            var result = await _httpClient.PutAsJsonAsync($"posts/{postId}/Tags", tagId);
            return result.IsSuccessStatusCode;
        }


    }
}
