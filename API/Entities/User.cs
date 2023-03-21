namespace API.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName {get; set;}
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt  {get; set; }
    }
}