using Microsoft.AspNetCore.Identity;

namespace HBlog.Domain.Entities
{
    public class AppUserRole : IdentityUserRole<Guid>
    {
        public User User { get; set; }
        public AppRole Role { get; set; }
        
    }
}