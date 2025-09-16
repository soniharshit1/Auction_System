using System;
using System.Collections.Generic;

namespace Auction_System_Library_Database.Models;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public int BuyerId { get; set; }

    public int AuctionId { get; set; }

    public decimal Amount { get; set; }

    public bool? PaymentStatus { get; set; }

    public DateTime? PaymentDate { get; set; }

    public int SellerId { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Auction Auction { get; set; } = null!;

    public virtual Person Buyer { get; set; } = null!;

    public virtual Person Seller { get; set; } = null!;
}
