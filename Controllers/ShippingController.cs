using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhoneVault.Controllers;
using PhoneVault.Models;
using PhoneVault.Services;

namespace PhoneVault.Controllers
{
    [Route("api/shipping")]
    [ApiController]
    public class ShippingController : ControllerBase
    {
        private readonly ShippingService shippingService;
        public ShippingController(ShippingService shippingService)
        {
            this.shippingService = shippingService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Shipping>>> GetAllShippingRecords()
        {
            var records= await shippingService.GetAllShippingRecords();
            if (records == null)
            {
                return NotFound();
            }
            return Ok(records);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Shipping>> GetShippingRecordById(int id)
        {
            var record=await shippingService.GetShippingRecordById(id);
            if (record == null)
            {
                return NotFound();
            }
            return Ok(record);
        }
        [HttpPost]
        public async Task<ActionResult> AddShippingRecord(Shipping record)
        {
            if(record == null)
            {
                return BadRequest();
            }
            await shippingService.AddShippingRecord(record);
            return Ok();
        }
        [HttpPut]
        public async Task<ActionResult> UpdateShippingRecord(Shipping record)
        {
            if( record == null)
            {
                return BadRequest();
            }
            await shippingService.UpdateShippingRecord(record);
            return Ok();
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteShippingRecord(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }
            await shippingService.DeleteShippingRecord(id);
            return Ok();
        }
    }
}
