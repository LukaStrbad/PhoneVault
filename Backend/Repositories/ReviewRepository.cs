using Microsoft.EntityFrameworkCore;
using PhoneVault.Data;
using PhoneVault.Models;

namespace PhoneVault.Repositories
{
    public class ReviewRepository: IReviewRepository
    {
        private readonly PhoneVaultContext _context;

        public ReviewRepository(PhoneVaultContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Review>> GetAllReviews() =>
            await _context.Reviews.ToListAsync();

        public async Task<Review> GetReviewById(int id) =>
            await _context.Reviews.FindAsync(id);

        public async Task AddReview(Review review)
        {
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateReview(Review review)
        {
            _context.Entry(review).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
            }
        }
    }
}
