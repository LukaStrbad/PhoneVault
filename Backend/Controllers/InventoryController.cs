using Microsoft.AspNetCore.Mvc;
using PhoneVault.Models;
using PhoneVault.Services;

namespace PhoneVault.Controllers
{
    [Route("api/inventory")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryService _inventoryService;
        public InventoryController(InventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inventory>>> GetAllInventoryRecords()
        {
            var records = await _inventoryService.GetAllInventoryRecords();
            if (records == null)
            {
                return NotFound();
            }
            return Ok(records);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Inventory>> GetInventoryRecordById(int id)
        {
            var record = await _inventoryService.GetInventoryRecordById(id);
            if (record == null)
            {
                return NotFound();
            }
            return Ok(record);
        }
        [HttpPost]
        public async Task<ActionResult> AddInventoryRecord(InventoryDTO record)
        {
            if (record == null)
            {
                return BadRequest();
            }
            await _inventoryService.AddInventoryRecord(record);
            return Ok(record);
        }
        [HttpPut]
        public async Task<ActionResult> UpdateInventoryRecord(Inventory record)
        {
            if (record == null)
            {
                return BadRequest();
            }
            await _inventoryService.UpdateInventoryRecord(record);
            return Ok(record);
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteInventoryRecord(int id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            await _inventoryService.DeleteInventoryRecord(id);
            return Ok();
        }
    }
}




