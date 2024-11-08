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
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts([FromQuery] string brand = null, [FromQuery] int? categoryId = null)
        {
            var products = await _productService.GetAllProductsAsync(brand, categoryId);
            return Ok(products);
        }


        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        //{
        //    var products = await _productService.GetAllProductsAsync();
        //    return Ok(products);
        //}

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            return product == null ? NotFound() : Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult> AddProduct([FromBody] ProductDTO productDto)
        {
            if(productDto == null)
            {
                return BadRequest();
            }
            await _productService.AddProductAsync(productDto);
            return Ok(productDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            product.Id = id;
            await _productService.UpdateProductAsync(product);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }
        
        [HttpGet("{id}/reviews")]
        public async Task<ActionResult<IEnumerable<ReviewResponse>>> GetReviews(int id)
        {
            var reviews = await _productService.GetReviewsByProductIdAsync(id);
            return Ok(reviews);
        }
        
        [HttpPost("{id}/reviews")]
        [Authorize("user")]
        public async Task<ActionResult> AddReview(int id, [FromBody] ReviewRequest review)
        {
            await _productService.AddReviewToProductAsync(id, review.Rating, review.Comment, User);
            return CreatedAtAction(nameof(GetReviews), new { id }, review);
        }
    }

}
