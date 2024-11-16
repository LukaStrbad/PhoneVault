using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneVault.Models;
using PhoneVault.Services;

namespace PhoneVault.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewsController : ControllerBase
{
    private readonly ReviewService _reviewService;

    public ReviewsController(ReviewService reviewService)
    {
        _reviewService = reviewService;
    }
        
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReviewResponse>>> GetReviews([FromQuery] int productId)
    {
        var reviews = await _reviewService.GetReviews(productId);
        return Ok(reviews);
    }
        
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> AddReview([FromQuery] int productId, [FromBody] ReviewRequest reviewRequest)
    {
        await _reviewService.AddReviewAsync(productId, reviewRequest.Rating, reviewRequest.Comment, User);
        return StatusCode(StatusCodes.Status201Created);
    }
    
    [HttpPut("{reviewId:int}")]
    [Authorize]
    public async Task<ActionResult> UpdateReview(int reviewId, [FromBody] ReviewRequest reviewRequest)
    {
        await _reviewService.UpdateReviewAsync(reviewId, reviewRequest.Rating, reviewRequest.Comment, User);
        return StatusCode(StatusCodes.Status204NoContent);
    }
    
    [HttpDelete("{reviewId:int}")]
    [Authorize]
    public async Task<ActionResult> DeleteReview(int reviewId)
    {
        await _reviewService.DeleteReviewAsync(reviewId, User);
        return StatusCode(StatusCodes.Status204NoContent);
    }
}