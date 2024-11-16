using PhoneVault.Models;
using PhoneVault.Repositories;

namespace PhoneVault.Services
{
    public class CategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> GetAllCategories() =>
            await _categoryRepository.GetAllCategories();
        public async Task<Category> GetCategoryById(int id) =>
            await _categoryRepository.GetCategoryById(id);
        public async Task AddCategoryAsync(CategoryDTO categoryDto)
        {
            if(categoryDto == null) {  throw new ArgumentNullException(nameof(categoryDto)); }
            var category = new Category
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description,
            };
            await _categoryRepository.AddCategory(category);
        }
        public async Task UpdateCategory(Category category) =>
            await _categoryRepository.UpdateCategory(category);

        public async Task DeleteCategory(int id) =>
            await _categoryRepository.DeleteCategory(id);

    }
}
