namespace HBlog.Contract.DTOs;

public class LoginDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
}
public record RefreshTokenDto(string? AccessToken, string? RefreshToken);