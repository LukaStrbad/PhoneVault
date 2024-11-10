using Microsoft.EntityFrameworkCore;
using PhoneVault.Data;
using PhoneVault.Models;
using PhoneVault.Repositories;
using System.Collections.Immutable;

namespace PhoneVault.Repositories
{
    public class ShoppingCartItemRepository : IShoppingCartItemRepository
    {
        private readonly PhoneVaultContext _context;
        public ShoppingCartItemRepository (PhoneVaultContext context)
        {
            _context = context;
        }
        public async Task AddShoppingCartItem(ShoppingCartItem shoppingCartItem)
        {
            if(shoppingCartItem == null)
            {
                throw new ArgumentNullException(nameof(shoppingCartItem));
            }
            await _context.ShoppingCartItems.AddAsync(shoppingCartItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteShoppingCartItem(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException();
            }
            var item = await _context.ShoppingCartItems.FindAsync(id);
            if (item==null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            _context.ShoppingCartItems.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ShoppingCartItem>> GetAllShoppingCartItems() =>
            await _context.ShoppingCartItems.ToListAsync();

        public async Task<ShoppingCartItem> GetShoppingCartItemById(int id) =>
            await _context.ShoppingCartItems.FindAsync(id);

        public async Task UpdateShoppingCartItem(ShoppingCartItem item)
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
