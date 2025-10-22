using Auction_System_Library_Database.Models;
using Auction_System_Library_Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction_System_Library_Infrastructure.Interfaces
{
    public interface IApprovalsRepository
    {
        Task<IEnumerable<Approval>> GetAllPendingApprovalAsync();
        Task<Approval?> AddApprovalAsync(int id);
        Task<Approval?> UpdateApprovalStatusAsync(int id, ApprovalDTO approvalDto);
        Task<Approval?> RejectApprovalAsync(int id, string remark);
    }
}
