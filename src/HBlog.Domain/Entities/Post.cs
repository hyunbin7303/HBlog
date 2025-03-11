using HBlog.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;
namespace HBlog.Domain.Entities
{
    [Table("Posts")]
    public class Post : BaseEntity<int>
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Desc { get; set; }
        public string Status { get; set; }
        public string Content { get; set; }
        public string LinkForPost {get ; set; }
        public string Type { get; set; } 
        public int Upvotes { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public virtual User User { get; set; }
        public virtual Category Category { get; set; }
        public List<Tag> Tags { get; set; } = [];

    }
}