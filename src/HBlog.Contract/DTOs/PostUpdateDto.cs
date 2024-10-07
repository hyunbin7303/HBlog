namespace HBlog.Contract.DTOs;
public class PostUpdateDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Desc { get; set; }
    public string Content { get; set; }
    public int CategoryId { get; set; }
    public string Type { get; set; }
    public string? LinkForPost { get; set; } = string.Empty;
    public int[] TagIds { get; set; } 
}
