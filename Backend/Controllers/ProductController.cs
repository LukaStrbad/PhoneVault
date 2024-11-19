using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneVault.Models;
using PhoneVault.Services;

namespace PhoneVault.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(int? categoryId)
        {
            var products = await _productService.GetAllProductsAsync(categoryId);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            return product == null ? NotFound() : Ok(product);
        }

        [HttpPost]
        [Authorize("admin")]
        public async Task<ActionResult> AddProduct(Product product)
        {
            await _productService.AddProductAsync(product);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        [Authorize("admin")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            product.Id = id;
            await _productService.UpdateProductAsync(product);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize("admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }
        
        [HttpPost("{id:int}/images")]
        [Authorize("admin")]
        public async Task<ActionResult> UpdateProductImages(int id, IEnumerable<string> urls)
        {
            await _productService.UpdateProductImages(id, urls);
            return Ok();
        }
        
        [HttpGet("{id:int}/images")]
        public async Task<ActionResult<IEnumerable<string>>> GetProductImages(int id)
        {
            var images = await _productService.GetProductImagesAsync(id);
            return Ok(images);
        }
    }

}
