using System.ComponentModel.DataAnnotations;

namespace HBlog.Contract.DTOs.OAuth;

public class OAuthCompleteDto
{
    [Required] public string OAuthTicket { get; set; } = string.Empty;
    [Required] public string UserName { get; set; } = string.Empty;
    [Required] public string FirstName { get; set; } = string.Empty;
    [Required] public string LastName { get; set; } = string.Empty;
    [Required] public string Email { get; set; } = string.Empty;
}
