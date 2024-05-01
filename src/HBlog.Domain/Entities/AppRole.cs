using Microsoft.AspNetCore.Identity;

namespace HBlog.Domain.Entities
{
    public class AppRole : IdentityRole<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}