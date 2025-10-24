using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction_System_Library_Infrastructure.DTOs
{
    public class CreateReviewDTO
    {
        // Rename SourceUserId to UserId/ReviewerId in your DTO for consistency, or adjust entity map.
        // Based on your controller logic, let's assume the entity expects 'UserId' for the reviewer.
        public int UserId { get; set; }  // The one giving the review (reviewerId/SourceUserId)
        public int TargetUserId { get; set; } // The one being reviewed (revieweeId/TargetUserId)
        public int AuctionId { get; set; } // Crucial for transaction context
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}
