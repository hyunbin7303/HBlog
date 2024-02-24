namespace KevBlog.Contract.DTOs;
public class PostCreateDto
{
    public string Title { get; set; }
    public string Desc { get; set; }
    public string Content { get; set; }
    public string LinkForPost { get; set; } = string.Empty;
    public string Type { get; set; }
}
