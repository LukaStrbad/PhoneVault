using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneVault.Controllers;
using PhoneVault.Models;
using PhoneVault.Services;

namespace PhoneVault.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ShoppingCartService _shoppingCartService;

        public ShoppingCartController(ShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShoppingCartItem>>> GetShoppingCart()
        {
            var shoppingCart = await _shoppingCartService.GetShoppingCart(User);
            if (shoppingCart is null)
            {
                await _shoppingCartService.AddShoppingCart(User);
                shoppingCart = await _shoppingCartService.GetShoppingCart(User);
            }

            return Ok(shoppingCart!.ShoppingCartItems);
        }

        [HttpPost]
        public async Task<ActionResult> AddShoppingCart(ShoppingCartDTO cart)
        {
            await _shoppingCartService.AddShoppingCart(User);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateShoppingCart(IEnumerable<ShoppingCartItem> shoppingCartItems)
        {
            var items = shoppingCartItems.ToList();
            items.ForEach(item => { item.Id = 0; });
            await _shoppingCartService.UpdateShoppingCart(User, items);
            return Ok();
        }
    }
}