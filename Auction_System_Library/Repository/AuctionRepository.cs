using Auction_System_Library.Data;
using Auction_System_Library.Interfaces;
using Auction_System_Library.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction_System_Library.Repository
{
    public class AuctionRepository : IAuctionRepository

    {
        private readonly AuctionDbContext _context;

        public AuctionRepository(AuctionDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Auction>> GetAllAuctionsAsync()
        {
            return await _context.Auctions.ToListAsync();
        }

        public async Task<IEnumerable<Auction>> GetActiveAuctionsAsync()
        {
            return await _context.Auctions
                .Where(a => a.Status == true && a.EndDate > DateTime.Now)
                .ToListAsync();
        }

        public async Task<Auction> GetAuctionByIdAsync(int id)
        {
            return await _context.Auctions
        }
    }
}
