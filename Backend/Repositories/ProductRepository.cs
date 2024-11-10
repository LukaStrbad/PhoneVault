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
        public async Task<IEnumerable<Product>> GetAllProducts(string brand = null, int? categoryId = null)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(brand))
            {
                query = query.Where(p => p.Brand == brand);
            }

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            return await query.ToListAsync();
        }

        //public async Task<IEnumerable<Product>> GetAllProducts() =>
        //    await _context.Products.ToListAsync();

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

        public async Task<IEnumerable<Review>> GetReviewsByProductId(int id)
        {
            var product = await _context.Products
                .Include(p => p.Reviews)
                .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(p => p.Id == id);
            return product?.Reviews ?? new List<Review>();
        }

        public async Task AddReviewToProduct(int productId, Review review)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                throw new Exception("Product not found");
            }
            
            product.Reviews.Add(review);
            await _context.SaveChangesAsync();
        }
    }

}
