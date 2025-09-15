using Auction_System_Library_Database.Data;
using Auction_System_Library_Database.Models;
using Auction_System_Library_Infrastructure.DTOs;
using Auction_System_Library_Infrastructure.Interfaces;
using Auction_System_Library_Infrastructure.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auction_System_WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewsController(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        // GET api/reviews/2/5
        [HttpGet("{userId:int}/{targetUserId:int}")]
        public async Task<IActionResult> GetByUserAndTarget(int userId, int targetUserId)
        {
            var reviews = await _reviewRepository.GetByUserAndTargetAsync(userId, targetUserId);
            if (!reviews.Any())
                return NotFound("No reviews found for this user and target.");

            return Ok(reviews);
        }

        //Get All reviews for admin
        [HttpGet]
        public async Task<IActionResult> GetAllReviews()
        {
            return Ok(await  _reviewRepository.GetAllReviewsAsync());
        }

        // POST api/reviews
        [HttpPost]
        public async Task<IActionResult> CreateReview(CreateReviewDTO dto)
        {
            var r = await _reviewRepository.AddReviewAsync(dto);
            if (r is null)
            {
                return BadRequest("Buyer or Seller doesn't exist");
            }
            return Ok("Review added successfully.");
        }

        // PUT api/reviews/10
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateReview(int id, UpdateReviewDTO dto)
        {
            var r = await _reviewRepository.UpdateReviewAsync(id, dto);
            if (r is null)
            {
                return NotFound("There is no review to update");
            }
            return Ok("Review updated successfully.");
        }

        // DELETE api/reviews/10
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var r = await _reviewRepository.DeleteReviewAsync(id);
            if (r is null)
            {
                return NotFound("There is no review to delete");
            }
            return Ok("Review deleted successfully.");
        }
    }
}
