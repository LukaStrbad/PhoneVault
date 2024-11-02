using PhoneVault.Models;
using PhoneVault.Repositories;

namespace PhoneVault.Services
{
    public class ReviewService
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<IEnumerable<Review>> GetAllReviewsAsync() =>
            await _reviewRepository.GetAllReviews();

        public async Task<Review> GetReviewByIdAsync(int id) =>
            await _reviewRepository.GetReviewById(id);

        public async Task AddReviewAsync(Review review) =>
            await _reviewRepository.AddReview(review);

        public async Task UpdateReviewAsync(Review review) =>
            await _reviewRepository.UpdateReview(review);

        public async Task DeleteReviewAsync(int id) =>
            await _reviewRepository.DeleteReview(id);
    }
}
