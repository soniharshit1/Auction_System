using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction_System_Library_Infrastucture.DTOs
{
    public class BidCreateDTO
    {

        public int AuctionId { get; set; }
        public int BuyerId { get; set; }
        public decimal Amount { get; set; }

    }
}
