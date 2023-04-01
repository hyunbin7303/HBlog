namespace API.Entities
{
    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}