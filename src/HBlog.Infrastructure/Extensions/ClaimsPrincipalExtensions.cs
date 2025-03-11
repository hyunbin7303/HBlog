using System.Security.Claims;
namespace HBlog.Infrastructure.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user) 
        {
            return user.FindFirst(ClaimTypes.Name)?.Value;
        }
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            return Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
}