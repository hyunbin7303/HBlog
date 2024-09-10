namespace HBlog.Contract.DTOs;
public class PostDisplayDto
{
    public int Id {get; set;}
    public string Title { get; set; } = string.Empty;
    public string Desc { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public int Upvotes { get; set; }
    public string UserName { get; set; } = string.Empty;
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    public IEnumerable<TagDto> Tags { get; set; } = null!;
}