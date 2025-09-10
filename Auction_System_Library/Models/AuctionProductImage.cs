using System;
using System.Collections.Generic;

namespace Auction_System_Library.Models;

public partial class AuctionProductImage
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int SellerId { get; set; }

    public int AuctionId { get; set; }

    public byte[]? ProductImages { get; set; }

    public virtual Auction Auction { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual Person Seller { get; set; } = null!;
}
