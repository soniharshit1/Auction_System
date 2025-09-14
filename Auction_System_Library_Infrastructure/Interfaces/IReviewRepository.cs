using Auction_System_Library_Database.Models;
using Auction_System_Library_Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction_System_Library_Infrastructure.Interfaces
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetAllReviewsAsync();
        Task<IEnumerable<Review?>> GetByUserAndTargetAsync(int userId, int targetUserId);
        Task <Review?>AddReviewAsync(CreateReviewDTO dto);
        Task <Review?> UpdateReviewAsync(int id, UpdateReviewDTO dto);
        Task <string?>DeleteReviewAsync(int id);
    }
}
