using KevBlog.Domain.Common;

namespace KevBlog.Domain.Entities
{
    public class FileStorage : BaseEntity
    {
        public string BucketName { get; set; }
        public string StorageType { get; set; }
        public int UserId { get; set; }
        public bool IsPublic { get; set; }
        public ICollection<User> SharedUsers { get; set; }
    }
}
