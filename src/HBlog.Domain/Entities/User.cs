

using Microsoft.AspNetCore.Identity;

namespace HBlog.Domain.Entities
{
    public class User : IdentityUser<int>
    {
        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTimeOffset LastActive { get; set; } = DateTime.Now;
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Introduction { get; set; }
        public string Interests { get; set; }
        public string LookingFor { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public List<Photo> Photos { get; set; } = new List<Photo>();
        public List<Post> Posts { get; set; } = new List<Post>();
        public List<UserLike> LikedByUsers {get; set;}
        public List<UserLike> LikedUsers {get; set; }
        public List<Message> MessagesSent { get; set; }
        public List<Message> MessagesReceived { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }

    }
}