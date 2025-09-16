using System;
using System.Collections.Generic;

namespace Auction_System_Library_Database.Models;

public partial class Bid
{
    public int BidId { get; set; }

    public int AuctionId { get; set; }

    public int BuyerId { get; set; }

    public decimal Amount { get; set; }

    public DateTime? BidTime { get; set; }

    public bool IsDeleted { get; set; } = false;

    public virtual Auction Auction { get; set; } = null!;

    public virtual Person Buyer { get; set; } = null!;
}
