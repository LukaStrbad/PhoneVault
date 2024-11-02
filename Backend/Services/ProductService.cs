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

        public async Task<IEnumerable<Product>> GetAllProductsAsync(string brand = null, int? categoryId = null)
        {
            return await _productRepository.GetAllProducts(brand, categoryId);
        }


        //public async Task<IEnumerable<Product>> GetAllProductsAsync() =>
        //    await _productRepository.GetAllProducts();

        public async Task<Product> GetProductByIdAsync(int id) =>
            await _productRepository.GetProductById(id);

        public async Task AddProductAsync(ProductDTO productDTO)
        {
            var product = new Product
            {
                Name = productDTO.Name,
                Brand = productDTO.Brand,
                Description = productDTO.Description,
                Specification = productDTO.Specification,
                NetPrice = productDTO.NetPrice,
                SellPrice = productDTO.SellPrice,
                QuantityInStock = productDTO.QuantityInStock,
                CategoryId = productDTO.CategoryId,
                UpdatedDate = DateTime.Now
            };
            await _productRepository.AddProduct(product);
        }
        public async Task UpdateProductAsync(Product product) =>
            await _productRepository.UpdateProduct(product);

        public async Task DeleteProductAsync(int id) =>
            await _productRepository.DeleteProduct(id);
    }

}
