using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneVault.Data;
using PhoneVault.Models;

namespace PhoneVault.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly DbContext _context;

        public ReviewsController(DbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Review>>> GetReview()
        //{
        //    return await _context.Reviews.ToListAsync();
        //}
    }
}
