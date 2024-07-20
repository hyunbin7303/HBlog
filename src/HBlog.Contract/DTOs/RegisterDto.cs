using System.ComponentModel.DataAnnotations;
namespace HBlog.Contract.DTOs;
public class RegisterDto
{
    [Required] public string UserName { get; set; }
    [Required] public string FirstName { get; set; }
    [Required] public string LastName { get; set; }
    [Required] public string Email { get; set; }
    [Required]
    [StringLength(20, MinimumLength =6)]
    public string Password { get; set; }
}