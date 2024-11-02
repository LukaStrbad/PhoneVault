using PhoneVault.Models;
using PhoneVault.Repositories;

namespace PhoneVault.Services
{
    public class ShoppingCartItemService
    {
        private readonly IShoppingCartItemRepository _shoppingCartItemRepository;
        public ShoppingCartItemService(IShoppingCartItemRepository shoppingCartItemRepository)
        {
            _shoppingCartItemRepository = shoppingCartItemRepository;
        }
        public async Task<IEnumerable<ShoppingCartItem>> GetAllShoopingCartItems() =>
            await _shoppingCartItemRepository.GetAllShoppingCartItems();
        public async Task<ShoppingCartItem> GetShoppingCartItemById(int id) =>
            await _shoppingCartItemRepository.GetShoppingCartItemById(id);

        public async Task AddShoppingCartItem(ShoppingCartItemDTO shoppingCartItemDTO)
        {
            if (shoppingCartItemDTO == null)
            {
                throw new ArgumentNullException(nameof(shoppingCartItemDTO));
            }
            var item = new ShoppingCartItem
            {
                CartId = shoppingCartItemDTO.CartId,
                ProductId = shoppingCartItemDTO.ProductId,
                Quantity = shoppingCartItemDTO.Quantity,
            };
            await _shoppingCartItemRepository.AddShoppingCartItem(item);
        }
        public async Task UpdateShoppingCartItem(ShoppingCartItem item)
        {
            if (item == null) { throw new ArgumentNullException(nameof(item)); }
            await _shoppingCartItemRepository.UpdateShoppingCartItem(item);
        }
        public async Task DeleteShoppingCartItem(int id)
        {
            if (id == 0) { throw new ArgumentNullException(nameof(id)); }
            await _shoppingCartItemRepository.DeleteShoppingCartItem(id);
        }
    }
}
