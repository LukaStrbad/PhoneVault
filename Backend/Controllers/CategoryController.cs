using Microsoft.AspNetCore.Mvc;
using PhoneVault.Models;
using PhoneVault.Services;

namespace PhoneVault.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;
        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategories();
            if (!(categories == null))
            {
                return Ok(categories);
            }
            else
            {
                return BadRequest();
            }

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>>GetCategoryByID(int id)
        {
            var category = await _categoryService.GetCategoryById(id);
            if (!(category == null))
            {
                return Ok(category);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddCategory(CategoryDTO category)
        {
            if(category == null)
            {
                return BadRequest();
            }
            await _categoryService.AddCategoryAsync(category);
            return Ok(category);
        }

        [HttpPut("{id}")]
        public async Task <ActionResult> UpdateCategory(Category category)
        {
            await _categoryService.UpdateCategory(category);
            return Ok(category);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            await _categoryService.DeleteCategory(id);
            return Ok("Category sucesfully deleted");
        }
    }
}