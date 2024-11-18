using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using PhoneVault.Data;
using PhoneVault.Models;
using PhoneVault.Repositories;
using PhoneVault.Services;

namespace PhoneVault.Services
{
    public class OrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly PhoneVaultContext _context;

        public OrderService(IOrderRepository orderRepository, PhoneVaultContext context)
        {
            _orderRepository = orderRepository;
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllOrders(ClaimsPrincipal claimsPrincipal)
        {
            var userId = claimsPrincipal.FindFirstValue("id")
                         ?? claimsPrincipal.FindFirstValue("user_id");

            if (userId is null)
            {
                throw new Exception("User not found");
            }

            return await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.UserId == userId).ToListAsync();
        }

        public async Task<Order?> GetOrderById(int id, ClaimsPrincipal claimsPrincipal)
        {
            var userId = claimsPrincipal.FindFirstValue("id")
                         ?? claimsPrincipal.FindFirstValue("user_id");

            if (userId is null)
            {
                throw new Exception("User not found");
            }

            return await _context.Orders.FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);
        }

        public async Task AddOrder(OrderDTO orderDto, ClaimsPrincipal claimsPrincipal)
        {
            var userId = claimsPrincipal.FindFirstValue("id")
                         ?? claimsPrincipal.FindFirstValue("user_id");

            if (userId is null)
            {
                throw new Exception("User not found");
            }

            var orderItems = orderDto.OrderItems;
            var orderIds = orderItems.Select(oi => oi.ProductId).ToList();

            var products = await _context.Products.Where(p => orderIds.Contains(p.Id)).ToListAsync();

            var totalPrice = products.Sum(p =>
            {
                var quantity = orderItems.First(oi => oi.ProductId == p.Id).Quantity;
                return p.SellPrice * quantity;
            });

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalPrice = totalPrice,
                Status = 0,
                PaymentMethod = "Credit Card",
                PaymentStatus = "Paid",
                ShippingAddress = orderDto.ShippingAddress,
                OrderItems = products.Select(p =>
                {
                    return new OrderItem
                    {
                        PriceAtPurchase = p.SellPrice,
                        Quantity = orderItems.First(oi => oi.ProductId == p.Id).Quantity,
                        ProductId = p.Id
                    };
                }).ToList()
            };

            await _orderRepository.AddOrder(order);

            // Update product stock
            foreach (var product in products)
            {
                var orderItem = orderItems.First(oi => oi.ProductId == product.Id);
                product.QuantityInStock -= orderItem.Quantity;
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrder(Order order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));
            await _orderRepository.UpdateOrder(order);
        }

        public async Task DeleteOrder(int id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            await _orderRepository.DeleteOrder(id);
        }
    }
}