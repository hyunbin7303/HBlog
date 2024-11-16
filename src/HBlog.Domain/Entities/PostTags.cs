namespace HBlog.Domain.Entities
{
    public class PostTags
    {
        public int PostId { get; set; }
        public int TagId { get; set; }
        public virtual Post Post { get; set; } = null!;
        public virtual Tag Tag { get; set; } = null!;
    }
}
