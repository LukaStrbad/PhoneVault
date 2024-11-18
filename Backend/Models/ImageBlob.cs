using System.ComponentModel.DataAnnotations;

namespace PhoneVault.Models;

public class ImageBlob
{ 
    [Key]
    [MaxLength(1024)]
    public required string Name { get; set; }
    public byte[] Bytes { get; set; } = []; 
}