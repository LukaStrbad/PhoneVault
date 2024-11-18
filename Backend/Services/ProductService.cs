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

        public async Task UpdateProductImages(int id, IEnumerable<string> urls)
        {
            var product = await _context.Products.FindAsync(id);
            if (product is null)
            {
                throw new Exception("Product not found");
            }

            await _context.ProductImages
                .Where(pi => pi.ProductId == id)
                .ExecuteDeleteAsync();

            var images = urls.Select(url => new Image { Url = url, ProductId = id });
            await _context.ProductImages.AddRangeAsync(images);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<string>> GetProductImagesAsync(int id)
        {
            var product = await _context.ProductImages
                .Where(pi => pi.ProductId == id)
                .ToListAsync();

            return product.Select(pi => pi.Url);
        }
    }
}