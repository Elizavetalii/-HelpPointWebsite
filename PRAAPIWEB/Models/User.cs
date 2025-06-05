using System.Text.Json.Serialization;

namespace PRAAPIWEB.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        [JsonIgnore] // Чтобы избежать цикла при сериализации/десериализации
        public List<Forumpost>? Forumposts { get; set; }

    }
}
