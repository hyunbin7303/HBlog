using System.ComponentModel.DataAnnotations.Schema;
namespace KevBlog.Domain.Entities
{
    [Table("PostTags")]
    public class PostTags
    {
        public int TagId { get; set; }
        public int PostId {get; set;}
        public Post Post {get; set;}
        public Tag Tag {get; set;}
    }
}