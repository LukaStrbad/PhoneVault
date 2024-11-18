using Microsoft.AspNetCore.Mvc;
using PhoneVault.Services;

namespace PhoneVault.Controllers;

[ApiController]
[Route("[controller]")]
public class ImageBlobController(ImageBlobService imageBlobService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> AddImages()
    {
        var files = Request.Form.Files;
        var blobNames = new List<string>();
        foreach (var file in files)
        {
            var fileName = file.FileName;
            var blobName = await imageBlobService.AddImage(fileName, file.OpenReadStream());
            blobNames.Add(blobName);
        }

        return Ok(blobNames);
    }
    
    [HttpGet("{name}")]
    public async Task<ActionResult> GetImage(string name)
    {
        var bytes = await imageBlobService.GetImage(name);
        if (bytes is null)
        {
            return NotFound("Image not found");
        }
        return File(bytes, "image/jpeg");
    }
}