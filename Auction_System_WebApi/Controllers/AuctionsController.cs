using Auction_System_Library_Database.Data;
using Auction_System_Library_Database.Models;
using Auction_System_Library_Infrastructure.DTOs;
using Auction_System_Library_Infrastructure.Interfaces;
using Auction_System_Library_Infrastructure.Repository;
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
    public class AuctionsController : ControllerBase
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IApprovalsRepository _approvalRepository;

        public AuctionsController(IAuctionRepository auctionRepository,IApprovalsRepository approvalsRepository)
        {
            _auctionRepository = auctionRepository;
            _approvalRepository = approvalsRepository;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Auction>>> GetAllAuctions()
        {
            var auctions = await _auctionRepository.GetAllAuctionsAsync();
            return Ok(auctions);
        }


        [HttpGet("Active")]
        public async Task<ActionResult<IEnumerable<Auction>>> GetActiveAuctions()
        {
            var auctions = await _auctionRepository.GetActiveAuctionsAsync();
            return Ok(auctions);
        }

        [HttpGet("LiveByProduct")]
        public async Task<ActionResult<IEnumerable<Auction>>> GetLiveAuctionsByProduct([FromQuery] LiveProductAuctionDTO dto)
        {
            var auctions = await _auctionRepository.GetLiveAuctionsByProductAsync(dto.ProductId);

            if (auctions == null || !auctions.Any())
                return NotFound("No live auctions found for the specified product.");

            return Ok(auctions);
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<AuctionDTO>> GetAuctionById(int id)
        {
            var auction = await _auctionRepository.GetAuctionByIdAsync(id);

            if (auction == null)
                return NotFound("Auction not found");

            var dto = new AuctionDTO
            {
                AuctionId = auction.AuctionId,
                ProductId = auction.ProductId,
                ProductName = auction.Product?.ProductName,
                SellerName = auction.Seller?.Name,
                SellerId = auction.SellerId,
                StartPrice = auction.StartPrice,
                StartDate = auction.StartDate,
                EndDate = auction.EndDate
            };

            return Ok(dto);
        }


        [HttpGet("Seller/{sellerId}")]
        public async Task<ActionResult<IEnumerable<Auction>>> GetAuctionsBySeller(int sellerId)
        {
            var auctions = await _auctionRepository.GetAuctionsBySellerAsync(sellerId);
            return Ok(auctions);
        }


        [HttpGet("Product/{productId}")]
        public async Task<ActionResult<IEnumerable<Auction>>> GetAuctionsByProduct(int productId)
        {
            var auctions = await _auctionRepository.GetAuctionsByProductAsync(productId);
            return Ok(auctions);
        }

        [HttpPost]
        public async Task<ActionResult<string>> CreateAuction([FromForm] AuctionCreateDTO auctionDto, [FromForm] IFormFileCollection images)
        {
            var attributes = auctionDto.Attributes.Select(attr => new AddAuctionProductAttributesDTO
            {
                AttributeId = attr.AttributeId,
                AttributeValue = attr.AttributeValue
            }).ToList();

            var imageDtos = images.Select(file => new TestDto { File = file }).ToList();

            var result = await _auctionRepository.CreateAuctionWithAttributesAsync(
                auctionDto.ProductId,
                auctionDto.SellerId,
                auctionDto.StartDate,
                auctionDto.EndDate,
                auctionDto.StartPrice,
                attributes,
                imageDtos
            );

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<string>> UpdateAuction(int id, [FromBody] AuctionUpdateDTO auctionDto)
        {
            var updatedAuction = new Auction()
            {
                ProductId = auctionDto.ProductId,
                StartPrice = auctionDto.StartPrice,
                StartDate = auctionDto.StartDate,
                EndDate = auctionDto.EndDate
            };

            var response = await _auctionRepository.UpdateAuctionAsync(id, updatedAuction);
            if (response == "Auction not found")
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPatch("{id}/Close")]
        public async Task<ActionResult<string>> CloseAuction(int id, [FromBody] AuctionCloseDTO closeDto)
        {
            // Call repository method. This creates the Transaction with PaymentStatus=0.
            var response = await _auctionRepository.CloseAuctionAsync(id, closeDto.FinalBid);

            if (response.Contains("Auction not found"))
            {
                return NotFound(response);
            }

            // Return the response, which tells the admin the transaction is pending (0)
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteAuction(int id)
        {
            var response = await _auctionRepository.DeleteAuctionAsync(id);
            if (response == "Auction not found")
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
