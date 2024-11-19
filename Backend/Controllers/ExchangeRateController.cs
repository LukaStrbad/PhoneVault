using Microsoft.AspNetCore.Mvc;

namespace PhoneVault.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExchangeRateController : ControllerBase
{
    private const string ExchangeRateApiUrl = "https://api.hnb.hr/tecajn-eur/v3";
    
    [HttpGet]
    public async Task<ActionResult> GetExchangeRate()
    {
        using var client = new HttpClient();
        var response = await client.GetAsync(ExchangeRateApiUrl);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return Ok(content);
        }
        return BadRequest();
    }
}