namespace HBlog.Domain.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public Guid SenderId { get; set; }
        public string SenderUsername { get; set; }
        public User Sender { get; set; }
        public Guid RecipientId { get; set; }
        public string RecipientUsername { get; set; }
        public User Recipient { get; set; }
        public string Content { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; } = DateTime.UtcNow;
        public bool SenderDeleted { get; set; }
        public bool RecipientDeleted { get; set; }
    }
}