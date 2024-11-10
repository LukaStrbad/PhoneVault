using Microsoft.EntityFrameworkCore;
using PhoneVault.Data;
using PhoneVault.Models;
using PhoneVault.Repositories;

namespace PhoneVault.Repositories
{
    public class OrderRepository: IOrderRepository
    {
        private readonly PhoneVaultContext _context;
        public OrderRepository(PhoneVaultContext context)
        {
            _context = context;
        }


        public async Task AddOrder(Order order)
        {
            if (order == null)
            { 
                throw new ArgumentNullException(nameof(order));
            }
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteOrder(int id)
        {
            if(id==null) throw new ArgumentNullException(nameof(id));
            var order=await _context.Orders.FindAsync(id);

            if(order==null) throw new ArgumentNullException(nameof(order));

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrders() =>
            await _context.Orders.ToListAsync();

        public async Task<Order> GetOrderById(int id) =>
            await _context.Orders.FindAsync(id);
        public async Task UpdateOrder(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}

