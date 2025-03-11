using HBlog.Domain.Common;
namespace HBlog.Domain.Entities;
public class Category : BaseEntity<int>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public virtual ICollection<Post> Posts { get; set; }
}
