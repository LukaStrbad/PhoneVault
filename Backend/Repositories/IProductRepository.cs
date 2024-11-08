using PhoneVault.Models;

namespace PhoneVault.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProducts(string brand = null, int? categoryId = null);
        Task<Product> GetProductById(int id);
        Task AddProduct(Product product);
        Task UpdateProduct(Product product);
        Task DeleteProduct(int id);
        Task<IEnumerable<Review>> GetReviewsByProductId(int id);
        Task AddReviewToProduct(int productId, Review review);
        
    }
}
