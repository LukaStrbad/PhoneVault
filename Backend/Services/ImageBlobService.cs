using Microsoft.EntityFrameworkCore;
using PhoneVault.Data;
using PhoneVault.Models;

namespace PhoneVault.Services;

public class ImageBlobService(PhoneVaultContext context)
{
    public async Task<string> AddImage(string fileName, Stream inputStream)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException("Name cannot be empty");
        }

        using var memoryStream = new MemoryStream();
        await inputStream.CopyToAsync(memoryStream);
        var bytes = memoryStream.ToArray();
        
        var uuid = Guid.NewGuid().ToString();
        var imageBlob = new ImageBlob
        {
            Name = $"{uuid}-{fileName}",
            Bytes = bytes
        };
        
        await context.ImageBlobs.AddAsync(imageBlob);
        await context.SaveChangesAsync();
        return imageBlob.Name;
    }

    public async Task<byte[]?> GetImage(string name)
    {
        var imageBlob = await context.ImageBlobs.FirstOrDefaultAsync(i => i.Name == name);
        
        return imageBlob?.Bytes;
    }
}