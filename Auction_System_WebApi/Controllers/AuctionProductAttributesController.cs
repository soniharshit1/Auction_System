using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Auction_System_Library_Database.Data;
using Auction_System_Library_Database.Models;

namespace Auction_System_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionProductAttributesController : ControllerBase
    {
        private readonly AuctionDbContext _context;

        public AuctionProductAttributesController(AuctionDbContext context)
        {
            _context = context;
        }

        // GET: api/AuctionProductAttributes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuctionProductAttribute>>> GetAuctionProductAttributes()
        {
            return await _context.AuctionProductAttributes.ToListAsync();
        }

        // GET: api/AuctionProductAttributes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuctionProductAttribute>> GetAuctionProductAttribute(int id)
        {
            var auctionProductAttribute = await _context.AuctionProductAttributes.FindAsync(id);

            if (auctionProductAttribute == null)
            {
                return NotFound();
            }

            return auctionProductAttribute;
        }

        // PUT: api/AuctionProductAttributes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuctionProductAttribute(int id, AuctionProductAttribute auctionProductAttribute)
        {
            if (id != auctionProductAttribute.Id)
            {
                return BadRequest();
            }

            _context.Entry(auctionProductAttribute).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuctionProductAttributeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/AuctionProductAttributes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AuctionProductAttribute>> PostAuctionProductAttribute(AuctionProductAttribute auctionProductAttribute)
        {
            _context.AuctionProductAttributes.Add(auctionProductAttribute);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAuctionProductAttribute", new { id = auctionProductAttribute.Id }, auctionProductAttribute);
        }

        // DELETE: api/AuctionProductAttributes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuctionProductAttribute(int id)
        {
            var auctionProductAttribute = await _context.AuctionProductAttributes.FindAsync(id);
            if (auctionProductAttribute == null)
            {
                return NotFound();
            }

            _context.AuctionProductAttributes.Remove(auctionProductAttribute);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AuctionProductAttributeExists(int id)
        {
            return _context.AuctionProductAttributes.Any(e => e.Id == id);
        }
    }
}
