using Microsoft.AspNetCore.Mvc;
using PhoneVault.Models;
using PhoneVault.Services;

namespace PhoneVault.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController: ControllerBase
    {
        private readonly OrderService orderService;
        public OrderController(OrderService orderService)
        {
            this.orderService = orderService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders()
        {
            var orders = await orderService.GetAllOrders();
            if (orders == null)
            {
                return NotFound();
            }
            return Ok(orders);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            var order = await orderService.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }
        [HttpPost]
        public async Task<ActionResult> AddOrder(Order order)
        {
            if (order == null)
            {
                return BadRequest();
            }
            await orderService.AddOrder(order);
            return Ok();
        }
        [HttpPut]
        public async Task<ActionResult> UpdateOrder(Order order)
        {
            if (order == null)
            {
                return BadRequest();
            }
            await orderService.UpdateOrder(order);
            return Ok();
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            if (id == 0)
            {
                BadRequest();
            }
            await orderService.DeleteOrder(id);
            return Ok();
        }
    }
}
