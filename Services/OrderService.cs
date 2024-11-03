using PhoneVault.Models;
using PhoneVault.Repositories;
using PhoneVault.Services;

namespace PhoneVault.Services
{
    public class OrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<IEnumerable<Order>> GetAllOrders() =>
            await _orderRepository.GetAllOrders();
        public async Task<Order> GetOrderById(int id) =>
            await _orderRepository.GetOrderById(id);

        public async Task AddOrder(Order order)
        {
            if(order == null) throw new ArgumentNullException(nameof(order));
            await _orderRepository.AddOrder(order);
        }
        public async Task UpdateOrder(Order order)
        {
            if(order == null) throw new ArgumentNullException(nameof (order));
            await _orderRepository.UpdateOrder(order);
        }
        public async Task DeleteOrder(int id)
        {
            if(id==null) throw new ArgumentNullException(nameof(id));
            await _orderRepository.DeleteOrder(id);
        }
    }
}
