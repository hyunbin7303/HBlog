using KevBlog.Domain.Common;
namespace KevBlog.Domain.Entities;
public class Category : BaseEntity
{
    public int CategoryId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public virtual HashSet<PostCategory> PostCategory { get; set; } = new();
}
