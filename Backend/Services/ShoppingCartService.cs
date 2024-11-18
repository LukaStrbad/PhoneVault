using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using PhoneVault.Data;
using PhoneVault.Models;

namespace PhoneVault.Services;

public class ShoppingCartService(PhoneVaultContext context)
{
    public async Task<ShoppingCart?> GetShoppingCart(ClaimsPrincipal claimsPrincipal)
    {
        var user = await GetUser(claimsPrincipal);
        return user.ShoppingCart;
    }

    public async Task AddShoppingCart(ClaimsPrincipal claimsPrincipal)
    {
        var user = await GetUser(claimsPrincipal);

        var cart = new ShoppingCart
        {
            ShoppingCartItems = []
        };

        user.ShoppingCart = cart;
        await context.SaveChangesAsync();
    }
    public async Task UpdateShoppingCart(ClaimsPrincipal claimsPrincipal, IEnumerable<ShoppingCartItem> shoppingCartItems)
    {
        var user = await GetUser(claimsPrincipal);
        user.ShoppingCart.ShoppingCartItems = shoppingCartItems.ToList();
        await context.SaveChangesAsync();
    }

    private async Task<User> GetUser(ClaimsPrincipal claimsPrincipal)
    {
        var userId = claimsPrincipal.FindFirstValue("id")
                     ?? claimsPrincipal.FindFirstValue("user_id");

        var user = await context.Users
            .Where(u => u.Id == userId)
            .Include(u => u.ShoppingCart).ThenInclude(c => c.ShoppingCartItems)
            .FirstOrDefaultAsync();

        if (user is null)
        {
            throw new Exception("User not found");
        }
        
        return user;
    }
}