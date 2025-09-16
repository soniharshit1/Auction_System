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
    public class BidsController(IBidRepository bidRepository) : ControllerBase
    {
        private readonly IBidRepository _bidRepository = bidRepository;

        [HttpPost]
        public async Task<IActionResult> PlaceBid([FromBody] BidCreateDTO bidDto)
        {
            var bid = new Bid
            {
                AuctionId = bidDto.AuctionId,
                BuyerId = bidDto.BuyerId,
                Amount = bidDto.Amount,
                BidTime = DateTime.Now
            };

            var response = await _bidRepository.PlaceBidAsync(bid);
            if (response.Contains("Bid must be"))
                return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("Auction/{auctionId}")]
        public async Task<ActionResult<IEnumerable<Bid>>> GetBidsByAuction(int auctionId)
        {
            var bids = await _bidRepository.GetBidsByAuctionAsync(auctionId);
            return Ok(bids);
        }

        [HttpGet("Buyer/{buyerId}")]
        public async Task<ActionResult<IEnumerable<Bid>>> GetBidsByBuyer(int buyerId)
        {
            var bids = await _bidRepository.GetBidsByBuyerAsync(buyerId);
            return Ok(bids);
        }

        [HttpGet("Highest/{auctionId}")]
        public async Task<ActionResult<Bid>> GetHighestBid(int auctionId)
        {
            var bid = await _bidRepository.GetHighestBidAsync(auctionId);
            if (bid == null)
                return NotFound("No bids found for this auction.");

            return Ok(bid);
        }

        [HttpGet("{bidId}")]
        public async Task<ActionResult<Bid>> GetBidById(int bidId)
        {
            var bid = await _bidRepository.GetBidByIdAsync(bidId);
            if (bid == null)
                return NotFound("Bid not found.");

            return Ok(bid);
        }

        [HttpDelete("{bidId}")]
        public async Task<IActionResult> DeleteBid(int bidId)
        {
            var response = await _bidRepository.DeleteBidAsync(bidId);
            if (response == "Bid not found.")
                return NotFound(response);

            return Ok(response);
        }
    }
}

