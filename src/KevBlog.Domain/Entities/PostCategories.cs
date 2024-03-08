using KevBlog.Domain.Common;
namespace KevBlog.Domain.Entities;
public class PostCategories
{
    public int PostId { get; set; }
    public int CategoryId { get; set; }
    public virtual Post Post { get; set; }
    public virtual Category Category { get; set; }
}
