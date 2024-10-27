using System.Text.Json.Serialization;

namespace PhoneVault.Models;

public record UserCredentials(
    [property: JsonPropertyName("email")]
    string Email,
    [property: JsonPropertyName("password")]
    string Password
);