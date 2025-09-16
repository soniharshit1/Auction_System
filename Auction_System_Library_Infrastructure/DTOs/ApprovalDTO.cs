using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction_System_Library_Infrastructure.DTOs
{
    public class ApprovalDTO
    {
        public int ApprovalId { get; set; }

        public int ProductId { get; set; }

        public DateTime? ApprovalDate { get; set; }

        public bool? Status { get; set; }

        public string? Remarks { get; set; }

        public int AgentId { get; set; }
    }
}
