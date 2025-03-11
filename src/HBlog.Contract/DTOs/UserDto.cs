namespace HBlog.Contract.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string PhotoUrl { get; set; }
    public int Age { get; set; }
    public string KnownAs { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTimeOffset LastActive { get; set; } = DateTime.UtcNow;
    public string Introduction { get; set; }
}