using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhoneVault.Models;

public class Review
{
    public int Id { get; set; }
    public int Rating { get; set; }
    [MaxLength(1024)] public string Comment { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int UserId { get; set; }
    [JsonIgnore] public User? User { get; set; }
    public int ProductId { get; set; }
    [JsonIgnore] public Product? Product { get; set; }
}