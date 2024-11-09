using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
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
    }

}
