using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhoneVault.Controllers;
using PhoneVault.Models;
using PhoneVault.Services;

namespace PhoneVault.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ShoppingCartService _shoppingCartService;
        public ShoppingCartController(ShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShoppingCart>>> GetAllShoppingCarts()
        {
            var carts = await _shoppingCartService.GetAllShoppingCarts();
            if (carts == null)
            {
                return NotFound();
            }
            return Ok(carts);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ShoppingCart>> GetShoppingCartById(int id)
        {
            var cart = await _shoppingCartService.GetShoppingCartById(id);
            if (cart == null)
            {
                return NotFound();
            }
            return Ok(cart);
        }
        [HttpPost]
        public async Task<ActionResult> AddShoppingCart(ShoppingCartDTO cart)
        {
            if (cart == null)
            {
                return BadRequest();
            }
            await _shoppingCartService.AddShoppingCart(cart);
            return Ok(cart);
        }
        [HttpPut]
        public async Task<ActionResult> UpdateShoppingCart(ShoppingCart shoppingCart)
        {
            if(shoppingCart == null)
            {
                return BadRequest();
            }
            await _shoppingCartService.UpdateShoppingCart(shoppingCart);
            return Ok(shoppingCart);
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteShoppingCart(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }
            await _shoppingCartService.DeleteShoppingCart(id);
            return Ok();
        }
    }
}
