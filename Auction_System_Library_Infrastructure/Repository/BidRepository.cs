using Auction_System_Library_Database.Data;
using Auction_System_Library_Database.Models;
using Auction_System_Library_Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction_System_Library_Infrastructure.Repository
{
    public class BidRepository : IBidRepository
    {
        private readonly AuctionDbContext _context;
        private readonly IEmailService _emailService;

        public BidRepository(AuctionDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<string> PlaceBidAsync(Bid bid)
        {
            var auction = await _context.Auctions.FindAsync(bid.AuctionId);
            if (auction == null)
            {
                return "Auction not found.";
            }


            if (auction.SellerId == bid.BuyerId)
            {
                return "Sellers cannot place bids on their own auctions.";
            }


            var highestBid = await _context.Bids
                .Where(b => b.AuctionId == bid.AuctionId)
                .OrderByDescending(b => b.Amount)
                .Select(b => b.Amount)
                .FirstOrDefaultAsync();

            var minimumValidBid = highestBid > 0 ? highestBid : auction.StartPrice;

            if (bid.Amount <= minimumValidBid)
            {
                return $"Bid must be greater than {(highestBid > 0 ? "the current highest bid" : "the starting price")} of {minimumValidBid}.";
            }

            _context.Bids.Add(bid);
            await _context.SaveChangesAsync();
            

            var buyer = await _context.People.FindAsync(bid.BuyerId);
            if (buyer != null)
            {
                await _emailService.SendSimpleEmailAsync(
                    buyer.Email,
                    "Bid Placed",
                    $"Hi {buyer.Name}, your bid of {bid.Amount} on auction ID {bid.AuctionId} has been successfully placed."
                );
            }
            return $"Bid of {bid.Amount} placed successfully.";


        }


        public async Task<IEnumerable<Bid>> GetBidsByAuctionAsync(int auctionId)
        {
            return await _context.Bids
                .Where(b => b.AuctionId == auctionId)
                .OrderByDescending(b => b.BidTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Bid>> GetBidsByBuyerAsync(int buyerId)
        {
            return await _context.Bids
                .Where(b => b.BuyerId == buyerId)
                .OrderByDescending(b => b.BidTime)
                .ToListAsync();
        }

        public async Task<Bid?> GetHighestBidAsync(int auctionId)
        {
            return await _context.Bids
                .Where(b => b.AuctionId == auctionId)
                .OrderByDescending(b => b.Amount)
                .FirstOrDefaultAsync();
        }

        public async Task<Bid?> GetBidByIdAsync(int bidId)
        {
            return await _context.Bids
                .FirstOrDefaultAsync(b => b.BidId == bidId);
        }

        public async Task<string> DeleteBidAsync(int bidId)
        {
            var bid = await _context.Bids.FindAsync(bidId);
            if (bid != null)
            {
                _context.Bids.Remove(bid);
                await _context.SaveChangesAsync();
                return $"Bid {bidId} deleted successfully.";
            }
            return "Bid not found.";
        }
    }
}

