using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhoneVault.Models;

public class Image
{
    public int Id { get; set; }
    [MaxLength(1024)] public required string Url { get; set; }
    [JsonIgnore] public Product? Product { get; set; }
    public int ProductId { get; set; }
}