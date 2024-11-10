using Microsoft.EntityFrameworkCore;
using PhoneVault.Data;
using PhoneVault.Models;

namespace PhoneVault.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly PhoneVaultContext _context;
        public OrderItemRepository(PhoneVaultContext context)
        {
            _context = context;
        }
        public async Task AddOrderItem(OrderItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            await _context.OrderItems.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderItem(int id)
        {
            if (id == 0)
            {
                throw new ArgumentNullException(nameof(id));
            }
            var item = await _context.OrderItems.FindAsync(id);
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
        }

        public async Task<IEnumerable<OrderItem>> GetAllOrderItems() =>
            await _context.OrderItems.ToListAsync();

        public async Task<OrderItem> GetOrderItemById(int id) =>
            await _context.OrderItems.FindAsync(id);

        public async Task UpdateOrderItem(OrderItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
