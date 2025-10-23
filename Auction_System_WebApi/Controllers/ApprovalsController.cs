using Auction_System_Library_Database.Models;
using Auction_System_Library_Infrastructure.DTOs;
using Auction_System_Library_Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auction_System_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApprovalsController : ControllerBase
    {
        private readonly IApprovalsRepository _approvalRepository;

        public ApprovalsController(IApprovalsRepository approvalRepository)
        {
            _approvalRepository = approvalRepository;
        }

        // GET: api/Approvals/AllPendingApprovals
        [HttpGet("AllPendingApprovals")]
        public async Task<ActionResult<IEnumerable<Approval>>> GetAllPendingApprovals()
        {
            var approvals = await _approvalRepository.GetAllPendingApprovalAsync();
            return Ok(approvals);
        }

        // GET: api/Approvals/AllApprovedApprovals
        [HttpGet("AllApprovedApprovals")]
        public async Task<ActionResult<IEnumerable<Approval>>> GetAllApprovedApprovals()
        {
            var approvals = await _approvalRepository.GetAllApprovedApprovalsAsync();
            return Ok(approvals);
        }

        // GET: api/Approvals/AllRejectedApprovals
        [HttpGet("AllRejectedApprovals")]
        public async Task<ActionResult<IEnumerable<Approval>>> GetAllRejectedApprovals()
        {
            var approvals = await _approvalRepository.GetAllRejectedApprovalsAsync();
            return Ok(approvals);
        }

        // GET: api/Approvals/All
        [HttpGet("All")]
        public async Task<ActionResult<IEnumerable<Approval>>> GetAllApprovals()
        {
            var approvals = await _approvalRepository.GetAllApprovalsAsync();
            return Ok(approvals);
        }

        // GET: api/Approvals/Auction/{auctionId}
        [HttpGet("Auction/{auctionId}")]
        public async Task<ActionResult<Approval>> GetApprovalByAuctionId(int auctionId)
        {
            var approval = await _approvalRepository.GetApprovalByAuctionIdAsync(auctionId);
            if (approval == null)
                return NotFound($"No approval found for Auction ID {auctionId}");

            return Ok(approval);
        }

        // POST: api/Approvals/Add
        [HttpPost("Add")]
        public async Task<IActionResult> AddApproval(int auctionId)
        {
            if (auctionId <= 0)
                return BadRequest("Valid auction ID is required.");

            var result = await _approvalRepository.AddApprovalAsync(auctionId);
            return Ok(result);
        }

        // PUT: api/Approvals/ApproveAuction/{auctionId}
        [HttpPut("ApproveAuction")]
        public async Task<IActionResult> ApproveAuctionAsync([FromBody] ApprovalDTO approvalDto)
        {
            int auctionId = approvalDto.AuctionId;
            if (approvalDto == null)
                return BadRequest("Approval data is invalid");

            var result = await _approvalRepository.ApproveAuctionAsync(approvalDto);
            if (result.StartsWith("No Auction"))
                return NotFound(result);

            return Ok(result);
        }

        // PUT: api/Approvals/Reject/{auctionId}?remark=reason
        [HttpPut("Reject/{auctionId}")]
        public async Task<IActionResult> RejectApprovalAsync(int auctionId, [FromQuery] string remark)
        {
            if (string.IsNullOrWhiteSpace(remark))
                return BadRequest("Remark is required.");

            var result = await _approvalRepository.RejectApprovalAsync(auctionId, remark);
            if (result.StartsWith("No Auction"))
                return NotFound(result);

            return Ok(result);
        }
    }
}
