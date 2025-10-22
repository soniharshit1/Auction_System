using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction_System_Library_Infrastructure.DTOs
{
    public class TransactionDTO
    {
        public int BuyerId { get; set; }

        public int AuctionId { get; set; }

        public decimal Amount { get; set; }

        public bool? PaymentStatus { get; set; }

        public DateTime? PaymentDate { get; set; }

        public int SellerId { get; set; }

    }
}
