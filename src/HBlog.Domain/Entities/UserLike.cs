namespace HBlog.Domain.Entities
{
    public class UserLike
    {
        public User SourceUser { get; set; }
        public int SourceUserId { get; set; }
        public User TargetUser { get; set; }
        public int TargetUserId { get; set; }
    }
}