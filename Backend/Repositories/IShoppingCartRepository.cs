using PhoneVault.Models;

namespace PhoneVault.Repositories
{
    public interface IShoppingCartRepository
    {
        Task<IEnumerable<ShoppingCart>> GetShoppingCarts();
        Task <ShoppingCart> GetShoppingCartById(int id);
        Task AddShoppingCart(ShoppingCart shoppingCart);
        Task DeleteShoppingCart(int id);
        Task UpdateShoppingCart(ShoppingCart shoppingCart);
    }
}
