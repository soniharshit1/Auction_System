using Auction_System_Library_Database.Data;
using Auction_System_Library_Database.Models;
using Auction_System_Library_Infrastructure.DTOs;
using Auction_System_Library_Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auction_System_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApprovalsController : ControllerBase
    {
        private readonly IApprovalsRepository _ApprovalRepository;

        public ApprovalsController(IApprovalsRepository ApprovalsRepository)
        {
            _ApprovalRepository = ApprovalsRepository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Approval>>> GetPendingApproval()
        {
            return Ok(await _ApprovalRepository.GetAllPendingApprovalAsync());

        }
        [HttpPut("ApprovalDirect/{id}")]
        public async Task<IActionResult> ApproveExistingApproval(int id)
        {
            var result = await _ApprovalRepository.AddApprovalAsync(id);
            if (result == null)
                return NotFound("Approval record not found");

            return Ok(new { message = "Approval updated successfully.", result });
        }
        [HttpPut]
        [Route("Approve/{id}")]
        public async Task<IActionResult> UpdateApprovalStatusAsync(int id, ApprovalDTO approvalDto)
        {
            if(approvalDto == null) return BadRequest("Approval Data is required");

            var approve = await _ApprovalRepository.UpdateApprovalStatusAsync(id, approvalDto);
            if (approve == null) return NotFound();

            return Ok(approve);

        }

        [HttpPut]
        [Route("Reject/{id}")]
        public async Task<IActionResult> RejectApprovalAsync(int id, [FromQuery] string remark)
        {

            if (string.IsNullOrWhiteSpace(remark))
            return BadRequest("Remark is required.");

            var reject = await _ApprovalRepository.RejectApprovalAsync(id, remark);
            if (reject == null) return NotFound($"Approval with ID {id} not found.");

            return Ok(reject);


        }


    }
}
