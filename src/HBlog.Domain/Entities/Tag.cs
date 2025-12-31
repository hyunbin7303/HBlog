using HBlog.Domain.Common;
namespace HBlog.Domain.Entities
{
    public class Tag : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Slug { get; set; }
        public List<Post> Posts{ get; } = [];
    }
}