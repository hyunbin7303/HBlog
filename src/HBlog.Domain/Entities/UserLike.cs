namespace HBlog.Domain.Entities
{
    public class UserLike
    {
        public User SourceUser { get; set; }
        public Guid SourceUserId { get; set; }
        public User TargetUser { get; set; }
        public Guid TargetUserId { get; set; }
    }
}