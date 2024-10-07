using HBlog.Domain.Entities;
using System.Net.Http.Json;
namespace HBlog.WebClient.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetCategories();
    }
    public class CategoryClientService:BaseService,ICategoryService
    {
        public CategoryClientService(HttpClient httpClient,ILogger<CategoryClientService> logger): base(httpClient,logger)
        {
        }

        public async Task<IEnumerable<Category>> GetCategories() => await _httpClient.GetFromJsonAsync<IEnumerable<Category>>($"Categories");
    }
}
