using System.ComponentModel.DataAnnotations;

namespace PhoneVault.Models;

public class UserRefreshToken
{
    [Key]
    public int Id { get; set; }
    
    [StringLength(256)]
    public required string Email { get; init; }
    
    public required string RefreshToken { get; set; }
    
    public required DateTime ExpiryTime { get; set; }
    
    public bool IsExpired => DateTime.UtcNow >= ExpiryTime;
}