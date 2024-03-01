using KevBlog.Domain.Entities;
namespace KevBlog.Domain.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    Task<IEnumerable<Category>> GetCategoriesAsync();
}
