using Microsoft.AspNetCore.Identity;

namespace HBlog.Domain.Entities
{
    public class AppRole : IdentityRole<Guid>
    {
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}