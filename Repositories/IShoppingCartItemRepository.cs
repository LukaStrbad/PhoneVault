using PhoneVault.Models;

namespace PhoneVault.Repositories
{
    public interface IShoppingCartItemRepository
    {
        Task<IEnumerable<ShoppingCartItem>> GetAllShoppingCartItems();
        Task<ShoppingCartItem> GetShoppingCartItemById(int id);
        Task AddShoppingCartItem(ShoppingCartItem shoppingCartItem);
        Task DeleteShoppingCartItem(int id);   
        Task UpdateShoppingCartItem(ShoppingCartItem item);
    }
}
