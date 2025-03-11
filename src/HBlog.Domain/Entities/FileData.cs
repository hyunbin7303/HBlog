using HBlog.Domain.Common;

namespace HBlog.Domain.Entities
{
    public class FileData : BaseEntity<int>
    {
        public int FileStorageId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public virtual FileStorage FileStorage { get; set; }
    }
}
