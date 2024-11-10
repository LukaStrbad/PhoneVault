using PhoneVault.Models;

namespace PhoneVault.Repositories
{
    public interface IOrderItemRepository
    {
        Task<IEnumerable<OrderItem>> GetAllOrderItems();
        Task<OrderItem> GetOrderItemById(int id);
        Task AddOrderItem(OrderItem item);
        Task UpdateOrderItem(OrderItem item);
        Task DeleteOrderItem(int id);
    }
}
