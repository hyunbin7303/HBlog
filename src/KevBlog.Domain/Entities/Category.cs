using KevBlog.Domain.Common;
namespace KevBlog.Domain.Entities;
public class Category : BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public List<PostCategories> PostCategories { get; set; } = new();
}
