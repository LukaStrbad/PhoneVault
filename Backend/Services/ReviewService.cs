using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using PhoneVault.Data;
using PhoneVault.Enums;
using PhoneVault.Models;

namespace PhoneVault.Services;

public class ReviewService(PhoneVaultContext context)
{
    public async Task<IEnumerable<ReviewResponse>> GetReviews(int productId)
    {
        var product = await context.Products
            .Include(p => p.Reviews).ThenInclude(review => review.User)
            .FirstOrDefaultAsync(p => p.Id == productId);
        if (product == null)
        {
            throw new Exception("Product not found");
        }

        return product.Reviews.Select(r => new ReviewResponse
        {
            Id = r.Id,
            Rating = r.Rating,
            Comment = r.Comment,
            CreatedAt = r.CreatedAt,
            UserName = r.User?.Name ?? "",
            UserId = r.UserId
        });
    }

    public async Task AddReviewAsync(int productId, int rating, string comment, ClaimsPrincipal claimsPrincipal)
    {
        var userId = claimsPrincipal.FindFirstValue("id");
        if (!int.TryParse(userId, out var idInt))
        {
            throw new Exception("Invalid user id");
        }

        var user = await context.Users.FindAsync(idInt);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        var review = new Review
        {
            Rating = rating,
            Comment = comment,
            User = user
        };

        var product = await context.Products.FindAsync(productId);
        if (product == null)
        {
            throw new Exception("Product not found");
        }

        product.Reviews.Add(review);
        await context.SaveChangesAsync();
    }

    public async Task DeleteReviewAsync(int id, ClaimsPrincipal claimsPrincipal)
    {
        var review = await context.Reviews.FindAsync(id);
        if (review == null)
        {
            throw new Exception("Review not found");
        }

        var userId = claimsPrincipal.FindFirstValue("id");
        if (!int.TryParse(userId, out var idInt))
        {
            throw new Exception("Invalid user id");
        }

        if (review.UserId != idInt && !claimsPrincipal.IsInRole(UserTypes.Admin))
        {
            throw new Exception("User is not the owner of the review");
        }

        context.Reviews.Remove(review);
        await context.SaveChangesAsync();
    }

    public async Task UpdateReviewAsync(int id, int rating, string comment, ClaimsPrincipal claimsPrincipal)
    {
        var review = await context.Reviews.FindAsync(id);
        if (review == null)
        {
            throw new Exception("Review not found");
        }

        var userId = claimsPrincipal.FindFirstValue("id");
        if (!int.TryParse(userId, out var idInt))
        {
            throw new Exception("Invalid user id");
        }

        if (review.UserId != idInt && !claimsPrincipal.IsInRole(UserTypes.Admin))
        {
            throw new Exception("User is not the owner of the review");
        }

        review.Rating = rating;
        review.Comment = comment;
        await context.SaveChangesAsync();
    }
}