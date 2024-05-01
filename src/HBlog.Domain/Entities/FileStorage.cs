using HBlog.Domain.Common;
namespace HBlog.Domain.Entities
{
    public class FileStorage : BaseEntity
    {
        public string BucketName { get; set; }
        public string StorageType { get; set; }
        public int UserId { get; set; }
        public bool IsPublic { get; set; }
        public ICollection<User> SharedUsers { get; set; }
        public ICollection<FileData> Files { get; set; }
    }
}
