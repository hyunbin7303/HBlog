using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
namespace KevBlog.Api.Controllers
{
    public class CategoriesController : BaseApiController
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoriesController(ICategoryRepository repository)
        {
            _categoryRepository = repository;
        }
        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<Category>>> Get() => Ok(await _categoryRepository.GetCategoriesAsync());
    }
}
