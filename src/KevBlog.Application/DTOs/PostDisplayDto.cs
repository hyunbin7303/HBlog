namespace KevBlog.Application.DTOs
{
    public class PostDisplayDto
    {
        public string Title { get; set; }
        public string Desc { get; set; }
        public string Status { get; set; }
        public string Content { get; set; }
        public string LinkForPost { get; set; }
        public string Type { get; set; }
        public int Upvotes { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}