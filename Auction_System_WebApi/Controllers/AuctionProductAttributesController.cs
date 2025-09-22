using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Auction_System_Library_Database.Data;
using Auction_System_Library_Database.Models;
using Auction_System_Library_Infrastructure.Interfaces;
using Auction_System_Library_Infrastructure.DTOs;

namespace Auction_System_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionProductAttributesController : ControllerBase
    {
        private readonly IAuctionProductAttributeRepository _auctionProductAttributeRepository;

        public AuctionProductAttributesController(IAuctionProductAttributeRepository auctionProductAttributeRepository)
        {
            _auctionProductAttributeRepository = auctionProductAttributeRepository;
        }

        // GET: api/AuctionProductAttributes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuctionProductAttribute>>> GetAuctionProductAttributes([FromQuery] int? productId, [FromQuery] int? auctionId)
        {
            if (productId == null || auctionId == null)
            {
                return BadRequest("Both productId and auctionId are required.");
            }

            var result = await _auctionProductAttributeRepository.GetAttributesForAuctionAsync(productId.Value, auctionId.Value);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<AuctionProductAttribute>> PostAuctionProductAttribute([FromQuery]int auctionId,[FromQuery]int attributeId, [FromQuery]string attributeValue)
        {
            var result = await _auctionProductAttributeRepository.SaveAttributesAsync(auctionId, attributeId, attributeValue);
            if(result == "Auction not found.")
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        // DELETE: api/AuctionProductAttributes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuctionProductAttribute(int id)
        {
            var result = await _auctionProductAttributeRepository.DeleteAsync(id);
            if(result == "Attribute not found or has been deleted.")
            {
                return NotFound(result);
            }
            return Ok(result);
        }

    }
}
