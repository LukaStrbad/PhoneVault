using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using PhoneVault.Models;
using PhoneVault.Repositories;
using PhoneVault.Services;

namespace PhoneVault.Services
{
    public class ShoppingCartService
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        public ShoppingCartService(IShoppingCartRepository shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
        }
        public async Task<IEnumerable<ShoppingCart>> GetAllShoppingCarts() =>
            await _shoppingCartRepository.GetShoppingCarts();

        public async Task<ShoppingCart> GetShoppingCartById(int id) =>
            await _shoppingCartRepository.GetShoppingCartById(id);

        public async Task AddShoppingCart(ShoppingCart cart)
        {
            if (cart == null)
            {
                throw new ArgumentNullException(nameof(cart));
            }
            await _shoppingCartRepository.AddShoppingCart(cart);
        }
        public async Task UpdateShoppingCart(ShoppingCart cart)
        {
            if(cart== null) throw new ArgumentNullException(nameof(cart));
            await _shoppingCartRepository.UpdateShoppingCart(cart);
        }
        public async Task DeleteShoppingCart(int id)
        {
            if(id==0) throw new ArgumentNullException(nameof(id));
            await _shoppingCartRepository.DeleteShoppingCart(id);
        }
    }
}