using System.ComponentModel.DataAnnotations;

namespace HBlog.Contract.DTOs.OAuth;

public class OAuthLoginDto
{
    [Required] public string IdToken { get; set; } = string.Empty;
    public string? RawNonce { get; set; }
    public string? AuthorizationCode { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
}
