using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace PhoneVault.Models
{
    public class User
    {
        [MaxLength(256)] [Key] public string Id { get; set; } = Guid.NewGuid().ToString();
        [MaxLength(256)] public required string Name { get; set; }
        [MaxLength(256)] public required string Email { get; set; }
        [MaxLength(1024)] public required string Password { get; set; }
        [MaxLength(64)] public string? PhoneNumber { get; set; }
        [MaxLength(256)] public string? Address { get; set; }
        [MaxLength(64)] public string? UserType { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UserAccountType AccountType { get; set; } = UserAccountType.Server;

        [JsonIgnore] public ICollection<Order> Orders { get; set; } = [];
        [JsonIgnore] public ShoppingCart? ShoppingCart { get; set; }

        [JsonIgnore] public ICollection<Review> Reviews { get; set; } = [];
    }
}