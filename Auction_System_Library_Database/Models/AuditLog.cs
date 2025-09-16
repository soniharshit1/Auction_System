using System;
using System.Collections.Generic;

namespace Auction_System_Library_Infrastructure.Models;

public partial class AuditLog
{
    public int LogId { get; set; }

    public string? TableName { get; set; }

    public int? RecordId { get; set; }

    public string? Action { get; set; }

    public string? PerformedBy { get; set; }

    public DateTime? Timestamp { get; set; }

    public string? Details { get; set; }

    public bool? IsDeleted { get; set; }
}
