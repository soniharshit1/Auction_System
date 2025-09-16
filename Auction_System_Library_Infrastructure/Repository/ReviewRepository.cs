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
            var checkReviewer = _context.Reviews.AnyAsync(p=>p.UserId == dto.UserId);
            if(checkReviewer is null)
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

        public async Task<string?> DeleteReviewAsync(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return null;
            review.IsDeleted = true;
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
            return $"{review.UserId} and with seller {review.TargetUserId} is deleted i.e isdeleted is 1 now.";

        }

        public async Task<IEnumerable<Review>> GetAllReviewsAsync()
        {
            return await _context.Reviews.ToListAsync();
        }

        public async Task<IEnumerable<Review?>> GetByUserAndTargetAsync(int userId, int targetUserId)
        {
            return await _context.Reviews
                .Where(r => r.UserId == userId && r.TargetUserId == targetUserId)
                .ToListAsync();
        }

        public async Task<Review?> UpdateReviewAsync(int id, UpdateReviewDTO dto)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return null;

            review.Rating = dto.Rating;
            review.Comment = dto.Comment;
            await _context.SaveChangesAsync();
            return review;
        }
    }
}
