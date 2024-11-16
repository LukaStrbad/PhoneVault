using Microsoft.EntityFrameworkCore;
using PhoneVault.Data;
using PhoneVault.Models;

namespace PhoneVault.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly PhoneVaultContext _context;

        public ProductRepository(PhoneVaultContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProducts() =>
            await _context.Products.ToListAsync();

        public async Task<IEnumerable<Product>> GetAllProducts(string? brand = null, int? categoryId = null)
        {
            return await _context.Products
                .Where(p => (brand == null || p.Brand == brand) && (!categoryId.HasValue || p.CategoryId == categoryId))
                .ToListAsync();
        }

        public async Task<Product?> GetProductById(int id) =>
            await _context.Products.FindAsync(id);

        public async Task AddProduct(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProduct(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }

}
