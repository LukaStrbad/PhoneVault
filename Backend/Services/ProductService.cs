using PhoneVault.Models;
using PhoneVault.Repositories;

namespace PhoneVault.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
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
    }

}
