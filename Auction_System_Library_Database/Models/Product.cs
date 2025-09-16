using System;
using System.Collections.Generic;

namespace Auction_System_Library_Database.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public int CategoryId { get; set; }

    public bool IsDeleted { get; set; } = false;

    public virtual ICollection<Approval> Approvals { get; set; } = new List<Approval>();

    public virtual ICollection<AuctionProductImage> AuctionProductImages { get; set; } = new List<AuctionProductImage>();

    public virtual ICollection<Auction> Auctions { get; set; } = new List<Auction>();

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<GeneralProductAttribute> GeneralProductAttributes { get; set; } = new List<GeneralProductAttribute>();
}
