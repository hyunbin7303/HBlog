namespace KevBlog.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName {get; set;}
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt  {get; set; }
        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string Interests { get; set; }
        public string LookingFor { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public List<Photo> Photos { get; set; } = new List<Photo>();
        public List<Post> Posts { get; set; } = new List<Post>();
    }
}