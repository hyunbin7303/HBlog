namespace KevBlog.Contract.DTOs;
public class PostDisplayDto
{
    public int Id {get; set;}
    public string Title { get; set; }
    public string Desc { get; set; }
    public string Content { get; set; }
    public int CategoryId { get; set; }
    public int Upvotes { get; set; }
    public string UserName { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}