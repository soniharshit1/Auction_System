using Auction_System_Library_Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auction_System_Library_Database.Models;
using Auction_System_Library_Infrastructure.DTOs;
using Auction_System_Library_Database.Data;
using Microsoft.EntityFrameworkCore;

namespace Auction_System_Library_Infrastructure.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AuctionDbContext _context;

        public ReviewRepository(AuctionDbContext context)
        {
            _context = context;
        }
        //reviewee - seller - the one who is being reviewed
        //reviewer - buyer - the one who reviews the seller
        public async Task<Review?> AddReviewAsync(CreateReviewDTO dto)
        {
            var checkReviewer = _context.Reviews.Where(p => p.IsDeleted != true).FirstOrDefaultAsync(p => p.UserId == dto.UserId);
            if (checkReviewer is null)
            {
                return null;
            }
            var checkReviewee = _context.Reviews.AnyAsync(p => p.TargetUserId == dto.TargetUserId);
            if (checkReviewee is null)
            {
                return null;
            }
            var review = new Review
            {
                UserId = dto.UserId,
                TargetUserId = dto.TargetUserId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                Date = DateTime.Now
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return review;
        }

        //public async Task<string?> DeleteReviewAsync(int id)
        //{
        //    var review = await _context.Reviews.Where(p => p.IsDeleted != true).FirstOrDefaultAsync(u => u.UserId == id); ;
        //    if (review == null) return null;
        //    review.IsDeleted = true;
        //    _context.Reviews.Update(review);
        //    await _context.SaveChangesAsync();
        //    return $"{review.UserId} and with seller {review.TargetUserId} is deleted i.e isdeleted is 1 now.";

        //}
        public async Task<string?> DeleteReviewAsync(int id)
        {
            // ✅ FIX: Search by ReviewId (assuming your Review model has a property named ReviewId or Id)
            // Use the appropriate property name for the review's primary key (e.g., u.ReviewId or u.Id)
            var review = await _context.Reviews
                                       .Where(p => p.IsDeleted != true)
                                       .FirstOrDefaultAsync(u => u.ReviewId == id); // <-- Assuming ReviewId is the correct property

            if (review == null) return null;

            review.IsDeleted = true;
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();

            // The return message can be simplified, but the logic is fixed
            return $"Review with ID {review.ReviewId} has been soft-deleted.";
        }

        public async Task<IEnumerable<Review>> GetAllReviewsAsync()
        {
            return await _context.Reviews.ToListAsync();
        }

        public async Task<IEnumerable<Review?>> GetByUserAndTargetAsync(int userId, int targetUserId)
        {
            return await _context.Reviews
                .Where(r => r.UserId == userId && r.TargetUserId == targetUserId && r.IsDeleted==false)
                .ToListAsync();
        }

        //public async Task<Review?> UpdateReviewAsync(int id, UpdateReviewDTO dto)
        //{
        //    var review = await _context.Reviews.Where(p => p.IsDeleted != true).FirstOrDefaultAsync(u => u.UserId == id); ;
        //    if (review == null) return null;

        //    review.Rating = dto.Rating;
        //    review.Comment = dto.Comment;
        //    await _context.SaveChangesAsync();
        //    return review;
        //}
        // Inside ReviewRepository.cs (or similar class)

        public async Task<Review?> UpdateReviewAsync(int reviewId, UpdateReviewDTO dto)
        {
            // STEP 1: Find the existing review in the database
            // This is the line that is returning null and causing the issue.
            var existingReview = await _context.Reviews
                .Where(p => p.IsDeleted != true)
                .FirstOrDefaultAsync(r => r.ReviewId == reviewId);

            if (existingReview is null)
            {
                // 🛑 PROBLEM: The review was not found by this ID.
                return null; // Triggers the "no review to update" error in the controller
            }

            // STEP 2: Update the entity properties
            existingReview.Rating = dto.Rating;
            existingReview.Comment = dto.Comment;

            // Optional: Update other fields if necessary, e.g., TargetUserId, AuctionId, etc.
            // existingReview.TargetUserId = dto.TargetUserId; 

            // STEP 3: Save changes
            await _context.SaveChangesAsync();

            return existingReview;
        }
        public async Task<IEnumerable<ReviewDTO>> GetReviewsForTargetUserAsync(int targetUserId)
        {
            var reviews = await _context.Reviews
                // Filter: where the TargetUserID column matches the requested personId
                .Where(r => r.TargetUserId == targetUserId)
                // Filter: exclude soft-deleted reviews
                .Where(r => r.IsDeleted == false)
                // Eagerly load the Reviewer's (User's) information to get their name
                .Include(r => r.User)
                // Project the results into the clean ReviewDTO structure
                .Select(r => new ReviewDTO
                {
                    ReviewId = r.ReviewId,
                    Rating = r.Rating,
                    UserId = r.UserId,
                    Comment = r.Comment,
                    Date = r.Date,
                    ReviewerName = r.User.Name

                })
                .OrderByDescending(r => r.Date) // Sort by most recent review
                .ToListAsync();

            return reviews;
        }
        public async Task<Review?> GetByIdAsync(int reviewId)
        {
            //var review = await _context.Reviews.FindAsync(reviewId);

            //        // 2. Check if the entity exists AND if it has NOT been soft-deleted.
            //        if (review != null && review.IsDeleted == false)
            //        {
            //            return review;
            //        }

            //        // Returns null if not found, or if it was marked as deleted.
            //        return null; - this code works, but we cant get fresh added reviews as it is find. 
            var review = await _context.Reviews
                .FirstOrDefaultAsync(r => r.ReviewId == reviewId && r.IsDeleted == false);
            return review;
        }
    }
}