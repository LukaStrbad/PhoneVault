using System.Text.Json.Serialization;

namespace PhoneVault.Models;

public class ReviewResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("rating")]
    public int Rating { get; set; }
    [JsonPropertyName("comment")]
    public string Comment { get; set; } = "";
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }
    [JsonPropertyName("userName")]
    public string UserName { get; set; }
    [JsonPropertyName("userId")]
    public string UserId { get; set; }
}