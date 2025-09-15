using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction_System_Library_Infrastructure.DTOs
{
    public class UpdateReviewDTO
    {
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
