using KevBlog.Domain.Common;

namespace KevBlog.Domain.Entities
{
    public class PostTags
    {
        public int PostId { get; set; }
        public int TagId { get; set; }
        public virtual Post Post { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
