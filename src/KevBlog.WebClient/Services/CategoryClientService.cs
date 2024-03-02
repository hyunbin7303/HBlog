using KevBlog.Domain.Entities;
using System.Net.Http.Json;
namespace KevBlog.WebClient.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetCategories();
    }
    public class CategoryClientService : ICategoryService
    {
        private HttpClient _httpClient;
        public CategoryClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Category>> GetCategories()
    => await _httpClient.GetFromJsonAsync<IEnumerable<Category>>($"Categories");
    }
}
