namespace WebChat.Entities
{
    public class Message
    {
        // Primary key
        public int Id { get; set; }

        // Foreign key properties for sender and receiver
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }

        // Navigation properties to Users
        public User? Sender { get; set; }
        public User? Receiver { get; set; }

        public string Text { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
        public string? FilePath { get; set; }
    }
}
