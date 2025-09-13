using System;
using System.Collections.Generic;

namespace Auction_System_Library_Database.Models;

public partial class Auction
{
    public int AuctionId { get; set; }

    public int ProductId { get; set; }

    public int SellerId { get; set; }

    public decimal StartPrice { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public decimal? FinalBid { get; set; }

    public bool? Status { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<AuctionProductAttribute> AuctionProductAttributes { get; set; } = new List<AuctionProductAttribute>();

    public virtual ICollection<AuctionProductImage> AuctionProductImages { get; set; } = new List<AuctionProductImage>();

    public virtual ICollection<Bid> Bids { get; set; } = new List<Bid>();

    public virtual Product Product { get; set; } = null!;

    public virtual Person Seller { get; set; } = null!;

    public virtual Transaction? Transaction { get; set; }
}
