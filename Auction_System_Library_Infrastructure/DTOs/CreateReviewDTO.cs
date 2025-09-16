using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction_System_Library_Infrastructure.DTOs
{
    public class CreateReviewDTO
    {
        public int UserId { get; set; }
        public int TargetUserId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
