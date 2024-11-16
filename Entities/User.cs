using System.Text.Json.Serialization;

namespace WebChat.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        // Foreign key for Role
        public int RoleId { get; set; }

        // Navigation property for Role
        public Role? Role { get; set; }

        // Navigation properties for Messages (Sender/Receiver)
        [JsonIgnore]
        public ICollection<Message>? SentMessages { get; set; }
        [JsonIgnore]
        public ICollection<Message>? ReceivedMessages { get; set; }

    }
}
