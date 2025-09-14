using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction_System_Library_Infrastucture.DTOs
{
    public class AuctionDTO
    {
        public int AuctionId { get; set; }
        public string? ProductName { get; set; }
        public string? SellerName { get; set; }
        public decimal StartPrice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

}
