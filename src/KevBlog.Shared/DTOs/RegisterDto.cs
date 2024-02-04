using System.ComponentModel.DataAnnotations;
namespace KevBlog.Contract.DTOs;
public class RegisterDto
{
    [Required] public string UserName { get; set; }
    [Required] public string FirstName { get; set; }
    [Required] public string LastName { get; set; }
    [Required] public string Email { get; set; }
    [Required] public string KnownAs { get; set; }
    public string Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    [Required]
    [StringLength(8, MinimumLength =4)]
    public string Password { get; set; }
}