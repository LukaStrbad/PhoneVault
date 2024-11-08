using PhoneVault.Models;

namespace PhoneVault.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product?> GetProductById(int id);
        Task AddProduct(Product product);
        Task UpdateProduct(Product product);
        Task DeleteProduct(int id);
        Task<IEnumerable<Review>> GetReviewsByProductId(int id);
        Task AddReviewToProduct(int productId, Review review);
        
    }
}
