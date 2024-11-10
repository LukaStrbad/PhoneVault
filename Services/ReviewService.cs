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

        public async Task AddReviewAsync(ReviewDTO reviewDto)
        {
            if(reviewDto == null) throw new ArgumentNullException(nameof(reviewDto));
            var review = new Review
            {
                UserId = reviewDto.UserId,
                ProductId = reviewDto.ProductId,
                Rating = reviewDto.Rating,
                Comment = reviewDto.Comment,
            };
    }

        public async Task UpdateReviewAsync(Review review) =>
            await _reviewRepository.UpdateReview(review);

        public async Task DeleteReviewAsync(int id) =>
            await _reviewRepository.DeleteReview(id);
    }
}
