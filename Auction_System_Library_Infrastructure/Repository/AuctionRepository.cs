using Auction_System_Library_Database.Data;
using Auction_System_Library_Infrastructure.Interfaces;
using Auction_System_Library_Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auction_System_Library_Infrastructure.Interface;

namespace Auction_System_Library_Infrastucture.Repository
{
    public class AuctionRepository : IAuctionRepository

    {
        private readonly AuctionDbContext _context;
        private readonly IEmailService _emailService;

        public AuctionRepository(AuctionDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
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
        public async Task<Auction?> GetAuctionByIdAsync(int id)
        {
            return await _context.Auctions
                .Include(a => a.Product)
                .Include(a => a.Seller)
                .FirstOrDefaultAsync(a => a.AuctionId == id);
        }

        public async Task<string> CreateAuctionsAsync(Auction auction)
        {
            _context.Auctions.Add(auction);
            await _context.SaveChangesAsync();

            var seller = await _context.People.FindAsync(auction.SellerId);
            if (seller != null)
            {
                await _emailService.SendSimpleEmailAsync(
                    seller.Email,
                    "Auction Created",
                    $"Hi {seller.Name}, your auction for product ID {auction.ProductId} has been successfully created."
                    );
            }

            return $"Auction for product {auction.ProductId} created successfully.";


        }


        public async Task<string> UpdateAuctionAsync(int id, Auction updatedAuction)
        {
            var existingAuction = await _context.Auctions.FindAsync(id);
            if (existingAuction != null)
            {
                existingAuction.StartPrice = updatedAuction.StartPrice;
                existingAuction.StartDate = updatedAuction.StartDate;
                existingAuction.EndDate = updatedAuction.EndDate;
                existingAuction.ProductId = updatedAuction.ProductId;

                _context.Auctions.Update(existingAuction);
                await _context.SaveChangesAsync();
                return $"Auction {id} updated successfully.";
            }
            return "Auction not found.";
        }


        public async Task<string> DeleteAuctionAsync(int id)
        {
            var auction = await _context.Auctions.FindAsync(id);
            if (auction != null)
            {
                _context.Auctions.Remove(auction);
                await _context.SaveChangesAsync();
                return $"Auction {id} deleted successfully.";
            }
            return "Auction not found.";
        }



        public async Task<IEnumerable<Auction>> GetAuctionsBySellerAsync(int sellerId)
        {
            return await _context.Auctions
                .Where(a => a.SellerId == sellerId)
                .ToListAsync();
        }


        public async Task<IEnumerable<Auction>> GetAuctionsByProductAsync(int productId)
        {
            return await _context.Auctions
                .Where(a => a.ProductId == productId)
                .ToListAsync();
        }


        public async Task<string> CloseAuctionAsync(int id, decimal finalBid)
        {
            var auction = await _context.Auctions.FindAsync(id);
            if (auction != null)
            {
                auction.Status = false;
                auction.FinalBid = finalBid;
                await _context.SaveChangesAsync();
                return $"Auction {id} closed with final bid {finalBid}.";
            }
            return "Auction not found.";
        }
    }
}
