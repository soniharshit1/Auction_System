using Auction_System_Library_Database.Data;
using Auction_System_Library_Database.Models;
using Auction_System_Library_Infrastructure.DTOs;
using Auction_System_Library_Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction_System_Library_Infrastructure.Repository
{
    public class AuctionRepository : IAuctionRepository

    {
        private readonly AuctionDbContext _context;
        private readonly ITransactionsRepository _transactionsRepository; // New Dependency
        private readonly IBidRepository _bidRepository;
        private readonly IEmailService _emailService;

        public AuctionRepository(AuctionDbContext context, ITransactionsRepository transactionsRepository, IBidRepository bidRepository, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
            _transactionsRepository = transactionsRepository;
            _bidRepository = bidRepository;
        }

        public async Task<IEnumerable<Auction>> GetAllAuctionsAsync()
        {
            //return await _context.Auctions.ToListAsync();
            // FIX: Use Include and ThenInclude to load related data
            return await _context.Auctions
                // 1. Include the Product entity
                .Include(a => a.Product)
                    // 2. Then, include the Category entity nested within the Product
                    .ThenInclude(p => p.Category)
                // 3. Optional: Filter out soft-deleted auctions (IsDeleted = false)
                .Where(a => a.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<IEnumerable<Auction>> GetActiveAuctionsAsync()
        {
            return await _context.Auctions
                .Where(a => a.Status == true && a.EndDate > DateTime.Now)
                .ToListAsync();
        }
        public async Task<IEnumerable<Auction>> GetLiveAuctionsByProductAsync(int productId)
        {
            return await _context.Auctions
                .Where(a => a.Status == true && a.EndDate > DateTime.Now && a.ProductId == productId)
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

            //var seller = await _context.People.FindAsync(auction.SellerId);
            //if (seller != null)
            //{
            //    await _emailService.SendSimpleEmailAsync(
            //        seller.Email,
            //        "Auction Created",
            //        $"Hi {seller.Name}, your auction for product ID {auction.ProductId} has been successfully created."
            //        );
            //}

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
                auction.IsDeleted = true;
                _context.Auctions.Update(auction);
                await _context.SaveChangesAsync();
                return $"Auction {id} marked as deleted successfully.";
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


        //public async Task<string> CloseAuctionAsync(int id, decimal finalBid)
        //{
        //    var auction = await _context.Auctions.FindAsync(id);
        //    if (auction != null)
        //    {
        //        auction.Status = false;
        //        auction.FinalBid = finalBid;
        //        await _context.SaveChangesAsync();
        //        return $"Auction {id} closed with final bid {finalBid}.";
        //    }
        //    return "Auction not found.";
        //}  

        // Auction_System_Library_Infrastructure.Repository/AuctionRepository.cs

        public async Task<string> CloseAuctionAsync(int auctionId, decimal finalBid)
        {
            var auction = await _context.Auctions.FirstOrDefaultAsync(a => a.AuctionId == auctionId && a.IsDeleted == false);

            if (auction == null)
            {
                return "Auction not found";
            }

            if (auction.Status == false)
            {
                return "Auction is already closed and cannot be re-opened.";
            }

            // 1. Get the Highest Bid (Winner)
            var highestBid = await _bidRepository.GetHighestBidAsync(auctionId);

            // Determine values for the transaction
            int? buyerId = highestBid?.BuyerId;
            decimal transactionAmount = highestBid != null ? highestBid.Amount : finalBid;
            int sellerId = auction.SellerId; // The seller ID from the auction

            // 2. Permanently Close the Auction
            auction.Status = false;
            auction.FinalBid = transactionAmount;
            _context.Auctions.Update(auction);

            // 3. Create Transaction with PaymentStatus = 0 (false)
            if (buyerId.HasValue && transactionAmount > 0)
            {
                // Check if a transaction already exists for this auction (prevents duplicates)
                var existingTransaction = await _transactionsRepository.GetTransactionByAuctionIdAsync(auctionId);
                if (existingTransaction != null)
                {
                    // If transaction exists, just save auction changes and exit.
                    await _context.SaveChangesAsync();
                    return "Auction closed. Transaction already exists.";
                }

                var transactionDto = new TransactionDTO
                {
                    AuctionId = auction.AuctionId,
                    BuyerId = buyerId.Value,
                    SellerId = sellerId,
                    Amount = transactionAmount,

                    // INITIAL STATE: PaymentStatus = 0 (false) and PaymentDate = null
                    PaymentStatus = false,
                    PaymentDate = null,

                    //TransactionDate = DateTime.UtcNow,
                };

                // This method should handle saving the new transaction to the database
                await _transactionsRepository.AddTransactionAsync(transactionDto);
            }
            // If no winner/bid and FinalBid is 0, no transaction is created.

            await _context.SaveChangesAsync();

            // Check if a transaction was actually created
            if (buyerId.HasValue && transactionAmount > 0)
            {
                return "Auction closed and **Pending** transaction recorded (PaymentStatus=0).";
            }
            return "Auction closed, no winning bid found to create a transaction.";
        }
    }
}
