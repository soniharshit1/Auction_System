using Auction_System_Library_Database.Data;
using Auction_System_Library_Infrastructure.Interfaces;
using Auction_System_Library_Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public async Task<IEnumerable<Approval>> GetAllPendingApprovalAsync()
        {
            var pendingApproval = await _context.Approvals.Where(a => a.IsDeleted == false)
                .Where(a => a.Status == null || a.Status == false).ToListAsync();

            return pendingApproval; 

        }
        public async Task<Approval?> AddApprovalAsync(int id)
        {
            var approval = await _context.Approvals.Where(p => p.IsDeleted == false).FirstOrDefaultAsync(p => p.ApprovalId == id);
            if (approval == null) return null;

            approval.Status = true;
            approval.ApprovalDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return approval;
        }
        public async Task<Approval?> UpdateApprovalStatusAsync(int id, ApprovalDTO approvalDto)
        {
            var approval = await _context.Approvals.Where(p => p.IsDeleted == false).FirstOrDefaultAsync(p => p.ApprovalId == id);
            if (approval == null) return null;
            approval.Status = approvalDto.Status;
            approval.ApprovalDate = approvalDto.ApprovalDate ?? DateTime.UtcNow;
            approval.Remarks = approvalDto.Remarks;
            approval.AgentId = approvalDto.AgentId;

            await _context.SaveChangesAsync();
            return approval;

        }

        public async Task<Approval?> RejectApprovalAsync(int id, string remark)
        {
            var approval = await _context.Approvals.Where(p => p.IsDeleted == false).FirstOrDefaultAsync(p=>p.ApprovalId == id);
            if (approval == null) return null;

            approval.Status = false;
            approval.Remarks = remark;
            approval.ApprovalDate= DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return approval;

            
        }

    }
}
