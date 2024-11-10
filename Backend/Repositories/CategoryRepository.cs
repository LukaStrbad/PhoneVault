using Microsoft.EntityFrameworkCore;
using PhoneVault.Data;
using PhoneVault.Models;

namespace PhoneVault.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly PhoneVaultContext _context;

        public CategoryRepository(PhoneVaultContext context)
        {
            _context = context;
        }
        public async Task AddCategory(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

            }
        }

        public async Task<IEnumerable<Category>> GetAllCategories() =>
                    await _context.Categories.ToListAsync();

        public async Task<Category> GetCategoryById(int id) =>
            await _context.Categories.FindAsync(id);

        public async Task UpdateCategory(Category category)
        {
            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

    }
}

