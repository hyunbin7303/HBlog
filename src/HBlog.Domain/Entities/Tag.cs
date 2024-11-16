using HBlog.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;
namespace HBlog.Domain.Entities
{
    [Table("Tags")]
    public class Tag : BaseEntity
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Slug { get; set; }
        //public List<PostTags> PostTags { get; } = [];
        public List<Post> Posts{ get; } = [];

    }
}