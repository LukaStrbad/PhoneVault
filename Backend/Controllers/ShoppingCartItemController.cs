using Microsoft.AspNetCore.Mvc;
using PhoneVault.Models;
using PhoneVault.Services;

namespace PhoneVault.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartItemController : ControllerBase
    {
        private readonly ShoppingCartItemService _shoppingCartItemService;
        public ShoppingCartItemController(ShoppingCartItemService shoppingCartItemService)
        {
            _shoppingCartItemService = shoppingCartItemService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShoppingCartItem>>> GetAllShoppingCartItems()
        {
            var items = await _shoppingCartItemService.GetAllShoopingCartItems();
            if (items == null)
            {
                return NotFound();
            }
            return Ok(items);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<ShoppingCartItem>>> GetShoppingCartItemById(int id)
        {
            var item=await _shoppingCartItemService.GetShoppingCartItemById(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }
        [HttpPost]
        public async Task<ActionResult> AddShoppingCartItem([FromBody] ShoppingCartItemDTO itemDto)
        {
            if(itemDto == null)
            {
                return BadRequest();
            }
            await _shoppingCartItemService.AddShoppingCartItem(itemDto);
            return Ok();
        }
        [HttpPut]
        public async Task<ActionResult> UpdateShoppingCartItem(ShoppingCartItem item)
        {
            if(item == null)
            {
                return BadRequest();
            }
            await _shoppingCartItemService.UpdateShoppingCartItem(item);
            return Ok();
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteShoppingCartItem(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }
            await _shoppingCartItemService.DeleteShoppingCartItem(id);
            return Ok();
        }
    }
}
