using System;
using System.Collections.Generic;

namespace Auction_System_Library_Database.Models;

public partial class Approval
{
    public int ApprovalId { get; set; }

    public int ProductId { get; set; }

    public DateTime? ApprovalDate { get; set; }

    public bool? Status { get; set; }

    public string? Remarks { get; set; }

    public int AgentId { get; set; }

    public bool IsDeleted { get; set; } = false;

    public virtual Person Agent { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
