using System;
using System.Collections.Generic;

namespace Auction_System_Library.Models;

public partial class GeneralProductAttribute
{
    public int AttributeId { get; set; }

    public int ProductId { get; set; }

    public string AttributeName { get; set; } = null!;

    public virtual ICollection<AuctionProductAttribute> AuctionProductAttributes { get; set; } = new List<AuctionProductAttribute>();

    public virtual Product Product { get; set; } = null!;
}
