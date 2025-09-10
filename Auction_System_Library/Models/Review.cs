using System;
using System.Collections.Generic;

namespace Auction_System_Library.Models;

public partial class Review
{
    public int ReviewId { get; set; }

    public int UserId { get; set; }

    public int TargetUserId { get; set; }

    public int Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime? Date { get; set; }

    public virtual Person TargetUser { get; set; } = null!;

    public virtual Person User { get; set; } = null!;
}
