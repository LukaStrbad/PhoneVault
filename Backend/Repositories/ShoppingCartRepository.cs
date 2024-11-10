using Microsoft.EntityFrameworkCore;
using PhoneVault.Data;
using PhoneVault.Models;
using PhoneVault.Repositories;

namespace PhoneVault.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly PhoneVaultContext _context;
        public ShoppingCartRepository(PhoneVaultContext context)
        {
            _context = context;
        }

        public async Task AddShoppingCart(ShoppingCart shoppingCart)
        {
            if (shoppingCart == null) 
            { 
                throw new ArgumentNullException(nameof(shoppingCart));
            }
            await _context.ShoppingCarts.AddAsync(shoppingCart);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteShoppingCart(int id)
        {
            if (id == 0)
            {
                throw new ArgumentNullException();
            }
            var cart = await _context.ShoppingCarts.FindAsync(id);
            if (cart == null) 
            {
                throw new ArgumentException(nameof(cart));
            }
            _context.ShoppingCarts.Remove(cart);
            await _context.SaveChangesAsync();
        }

        public async Task<ShoppingCart> GetShoppingCartById(int id) =>
            await _context.ShoppingCarts.FindAsync(id);

        public async Task<IEnumerable<ShoppingCart>> GetShoppingCarts() =>
            await _context.ShoppingCarts.ToListAsync();

        public async Task UpdateShoppingCart(ShoppingCart shoppingCart)
        {
            if(shoppingCart == null)
            {
                throw new ArgumentNullException(nameof(shoppingCart));
            }
            _context.Entry(shoppingCart).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
