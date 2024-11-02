using PhoneVault.Models;

namespace PhoneVault.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>>GetAllCategories();
        Task<Category>GetCategoryById(int id);
        Task AddCategory(Category category);
        Task DeleteCategory(int id);   

        Task UpdateCategory(Category category);

    }
}
