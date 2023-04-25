using Microsoft.AspNetCore.Identity;

namespace KevBlog.Domain.Entities
{
    public class AppUserRole : IdentityUserRole<int>
    {
        public User User { get; set; }
        public AppRole Role { get; set; }
        
    }
}