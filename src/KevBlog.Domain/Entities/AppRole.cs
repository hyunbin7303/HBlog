using Microsoft.AspNetCore.Identity;

namespace KevBlog.Domain.Entities
{
    public class AppRole : IdentityRole<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}