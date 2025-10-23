using Auction_System_Library_Database.Models;
using Auction_System_Library_Infrastructure.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auction_System_Library_Infrastructure.Interfaces
{
    public interface IApprovalsRepository
    {
        Task<IEnumerable<Approval>> GetAllApprovalsAsync();
        Task<IEnumerable<Approval>> GetAllPendingApprovalAsync();
        Task<IEnumerable<Approval>> GetAllApprovedApprovalsAsync();
        Task<IEnumerable<Approval>> GetAllRejectedApprovalsAsync();
        Task<Approval?> GetApprovalByAuctionIdAsync(int auctionId);
        Task<string> AddApprovalAsync(int auctionId);
        Task<string> ApproveAuctionAsync(ApprovalDTO approvalDto);
        Task<string> RejectApprovalAsync(int auctionId, string remark);
    }
}
