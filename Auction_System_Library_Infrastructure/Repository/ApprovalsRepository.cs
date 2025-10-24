using Auction_System_Library_Database.Data;

using Auction_System_Library_Infrastructure.Interfaces;

using Auction_System_Library_Database.Models;

using Microsoft.EntityFrameworkCore;

using Auction_System_Library_Infrastructure.DTOs;

namespace Auction_System_Library_Infrastructure.Repository

{

    public class ApprovalsRepository : IApprovalsRepository

    {

        private readonly AuctionDbContext _context;

        public ApprovalsRepository(AuctionDbContext context)

        {

            _context = context;

        }

        private IQueryable<Approval> ActiveApprovals()

        {

            return _context.Approvals

                .Where(approval => !approval.IsDeleted);

        }

        public async Task<IEnumerable<Approval>> GetAllApprovalsAsync()

        {

            return await ActiveApprovals().ToListAsync();

        }

        public async Task<IEnumerable<Approval>> GetAllPendingApprovalAsync()

        {

            return await ActiveApprovals()

                .Where(approval =>

                    approval.Status == false &&

                    approval.Remarks.ToLower() == "pending")

                .ToListAsync();

        }

        public async Task<IEnumerable<Approval>> GetAllApprovedApprovalsAsync()

        {

            return await ActiveApprovals()

                .Where(approval => approval.Status == true)

                .ToListAsync();

        }

        public async Task<IEnumerable<Approval>> GetAllRejectedApprovalsAsync()

        {

            return await ActiveApprovals()

                .Where(approval =>

                    approval.Status == false &&

                    approval.Remarks.ToLower() != "pending")

                .ToListAsync();

        }

        public async Task<Approval?> GetApprovalByAuctionIdAsync(int auctionId)

        {

            return await ActiveApprovals()

                .FirstOrDefaultAsync(approval => approval.AuctionId == auctionId);

        }

        public async Task<string> AddApprovalAsync(int auctionId)

        {

            var auction = await _context.Auctions

                .Where(a => !a.IsDeleted && a.AuctionId == auctionId)

                .FirstOrDefaultAsync();

            if (auction == null) return "Auction not found";

            var approval = new Approval

            {

                AuctionId = auctionId,

                ProductId = auction.ProductId,

                Status = false,

                ApprovalDate = DateTime.Now,

                Remarks = "pending",

                AgentId = 6,

                IsDeleted = false

            };

            await _context.Approvals.AddAsync(approval);

            await _context.SaveChangesAsync();

            return "Auction sent for approval";

        }

        public async Task<string> ApproveAuctionAsync(ApprovalDTO approvalDto)

        {

            int auctionId = approvalDto.AuctionId;

            var approval = await ActiveApprovals()

                .FirstOrDefaultAsync(a => a.AuctionId == auctionId);

            if (approval == null)

                return $"No approval record found for auction ID {auctionId}";

            if (approval.Status == true)

                return $"Auction ID {auctionId} is already approved";

            if (!string.Equals(approval.Remarks, "pending", StringComparison.OrdinalIgnoreCase))

                return $"Auction ID {auctionId} cannot be approved as it is not in pending state";

            approval.Status = true;

            approval.Remarks = approvalDto.Remarks;

            approval.ApprovalDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return $"Auction ID {auctionId} approved successfully";

        }


        public async Task<string> RejectApprovalAsync(int auctionId, string remark)

        {

            var approval = await ActiveApprovals()

                .FirstOrDefaultAsync(a => a.AuctionId == auctionId);

            if (approval == null)

                return $"No approval record found for auction ID {auctionId}";

            if (approval.Status == true)

                return $"Auction ID {auctionId} is already approved and cannot be rejected";

            if (!string.Equals(approval.Remarks, "pending", StringComparison.OrdinalIgnoreCase))

                return $"Auction ID {auctionId} cannot be rejected as it is not in pending state";

            approval.Status = false;

            approval.Remarks = remark;

            approval.ApprovalDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return $"Auction ID {auctionId} rejected successfully";

        }

    }

}
