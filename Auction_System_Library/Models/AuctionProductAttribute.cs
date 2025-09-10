using System;
using System.Collections.Generic;

namespace Auction_System_Library.Models;

public partial class AuctionProductAttribute
{
    public int Id { get; set; }

    public int AuctionId { get; set; }

    public string AttributeName { get; set; } = null!;

    public string AttributeValue { get; set; } = null!;

    public int AttributeId { get; set; }

    public virtual GeneralProductAttribute Attribute { get; set; } = null!;

    public virtual Auction Auction { get; set; } = null!;
}
