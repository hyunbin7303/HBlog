using KevBlog.Domain.Common;
namespace KevBlog.Domain.Entities
{
    public class Tag : BaseEntity
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Slug { get; set; }
        public List<Post> Posts { get; set; } = new();

    }
}