using System.Text.Json.Serialization;

namespace WebChat.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Navigation property for Users who have this Role
        [JsonIgnore]
        public ICollection<User>? Users { get; set; }
    }
}
