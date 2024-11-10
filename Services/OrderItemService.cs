using PhoneVault.Models;
using PhoneVault.Repositories;
using PhoneVault.Services;

namespace PhoneVault.Services
{
    public class OrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;
        public OrderItemService(IOrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }   
        public async Task<IEnumerable<OrderItem>> GetAllOrderItems() =>
            await _orderItemRepository.GetAllOrderItems();
        public async Task<OrderItem> GetOrderItemById(int id) =>
            await _orderItemRepository.GetOrderItemById(id);
        public async Task AddOrderItem(OrderItemDTO itemDTO)
        {
            if (itemDTO == null)
            {
                throw new ArgumentNullException(nameof(itemDTO));
            }
            var item = new OrderItem
            {
                OrderId = itemDTO.OrderId,
                ProductId= itemDTO.ProductId,
                Quantity = itemDTO.Quantity,
                PriceAtPurchase = itemDTO.PriceAtPurchase,
            };
            await _orderItemRepository.AddOrderItem(item);
        }
        public async Task UpdateOrderItem(OrderItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            await _orderItemRepository.UpdateOrderItem(item);
        }
        public async Task DeleteOrderItem(int id)
        {
            if (id == 0)
            {
                await _orderItemRepository.DeleteOrderItem(id);
            }
        }
    }
}
