using Microsoft.AspNetCore.Mvc;
using PhoneVault.Models;
using PhoneVault.Services;

namespace PhoneVault.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController: ControllerBase
    {
        private readonly OrderItemService _orderItemService;
        public OrderItemController(OrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetAllOrderItems()
        {
            var orderItems=await _orderItemService.GetAllOrderItems();
            if (orderItems == null) 
            { 
                return NotFound();
            }
            return Ok(orderItems);

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItem>> GetOrderItemById(int id)
        {
            var orderItem = await _orderItemService.GetOrderItemById(id);
            if (orderItem == null)
            {
                return NotFound();
            }
            return Ok(orderItem);
        }
        [HttpPost]
        public async Task<ActionResult> AddOrderItem([FromBody] OrderItemDTO itemDTO)
        {
            if (itemDTO == null)
            {
                return BadRequest();
            }
            await _orderItemService.AddOrderItem(itemDTO);
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrderItem(OrderItem orderItem)
        {
            if(orderItem == null)
            {
                return BadRequest();
            }
            await _orderItemService.UpdateOrderItem(orderItem);
            return Ok();
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }
            await _orderItemService.DeleteOrderItem(id);
            return Ok();
        }
    }
}

