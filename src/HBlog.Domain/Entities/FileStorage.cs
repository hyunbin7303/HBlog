using HBlog.Domain.Common;
namespace HBlog.Domain.Entities
{
    public class FileStorage : BaseEntity<int>
    {
        public string BucketName { get; set; }
        public string StorageType { get; set; }
        public Guid UserId { get; set; }
        public bool IsPublic { get; set; }
        public ICollection<User> SharedUsers { get; set; }
        public ICollection<FileData> Files { get; set; }
    }
}
