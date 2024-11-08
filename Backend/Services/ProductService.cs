using System.Security.Claims;
using PhoneVault.Data;
using PhoneVault.Models;
using PhoneVault.Repositories;

namespace PhoneVault.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly PhoneVaultContext _context;

        public ProductService(IProductRepository productRepository, PhoneVaultContext context)
        {
            _productRepository = productRepository;
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync() =>
            await _productRepository.GetAllProducts();

        public async Task<Product> GetProductByIdAsync(int id) =>
            await _productRepository.GetProductById(id);

        public async Task AddProductAsync(Product product) =>
            await _productRepository.AddProduct(product);

        public async Task UpdateProductAsync(Product product) =>
            await _productRepository.UpdateProduct(product);

        public async Task DeleteProductAsync(int id) =>
            await _productRepository.DeleteProduct(id);

        public async Task<IEnumerable<ReviewResponse>> GetReviewsByProductIdAsync(int id)
        {
            var reviews = await _productRepository.GetReviewsByProductId(id);
            return reviews.Select(r => new ReviewResponse
            {
                Id = r.Id,
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt,
                UserName = r.User?.Name
            });
        }
        
        public async Task AddReviewToProductAsync(int productId, int rating, string comment, ClaimsPrincipal claimsPrincipal)
        {
            var userId = claimsPrincipal.FindFirstValue("id");
            if (!int.TryParse(userId, out var idInt))
            {
                throw new Exception("Invalid user id");
            }
            var user = await _context.Users.FindAsync(idInt);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var review = new Review
            {
                Rating = rating,
                Comment = comment,
                User = user
            };
            
            await _productRepository.AddReviewToProduct(productId, review);
        }
    }

}
