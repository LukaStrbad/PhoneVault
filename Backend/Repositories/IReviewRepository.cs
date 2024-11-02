using PhoneVault.Models;

namespace PhoneVault.Repositories
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetAllReviews();
        Task<Review> GetReviewById(int id);
        Task AddReview(Review review);
        Task UpdateReview(Review review);
        Task DeleteReview(int id);
    }
}
