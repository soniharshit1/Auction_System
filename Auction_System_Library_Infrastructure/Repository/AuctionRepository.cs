using Auction_System_Library_Database.Data;
using Auction_System_Library_Database.Models;
using Auction_System_Library_Infrastructure.DTOs;
using Auction_System_Library_Infrastructure.Interfaces;
using Microsoft.Data.SqlClient;
using Auction_System_Library_Infrastructure.DTOs;
using Auction_System_Library_Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Auction_System_Library_Infrastructure.Repository
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
                .Where(a => a.Status == true && a.EndDate > DateTime.Now && !a.IsDeleted)
                .ToListAsync();
        }
        public async Task<Auction?> GetAuctionByIdAsync(int id)
        {
            return await _context.Auctions
                .Include(a => a.Product)
                .Include(a => a.Seller)
                .FirstOrDefaultAsync(a => a.AuctionId == id && !a.IsDeleted);
        }

        public async Task<string> CreateAuctionWithAttributesAsync(
        int productId,
        int sellerId,
        DateTime startDate,
        DateTime endDate,
        decimal startPrice,
        List<AddAuctionProductAttributesDTO> attributes,
        List<TestDto> images)
        {
            using var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = "CreateAuctionWithAttributes";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@ProductId", productId));
            command.Parameters.Add(new SqlParameter("@SellerId", sellerId));
            command.Parameters.Add(new SqlParameter("@StartDate", startDate));
            command.Parameters.Add(new SqlParameter("@EndDate", endDate));
            command.Parameters.Add(new SqlParameter("@StartPrice", startPrice));

            // Attributes TVP
            var attrTable = new DataTable();
            attrTable.Columns.Add("AttributeId", typeof(int));
            attrTable.Columns.Add("AttributeValue", typeof(string));

            foreach (var attr in attributes)
            {
                attrTable.Rows.Add(attr.AttributeId, attr.AttributeValue);
            }

            var attrParam = new SqlParameter("@Attributes", attrTable)
            {
                SqlDbType = SqlDbType.Structured,
                TypeName = "AuctionAttributeType"
            };
            command.Parameters.Add(attrParam);

            // Images TVP
            var imgTable = new DataTable();
            imgTable.Columns.Add("ProductId", typeof(int));
            imgTable.Columns.Add("SellerId", typeof(int));
            imgTable.Columns.Add("ProductImages", typeof(byte[]));

            foreach (var img in images)
            {
                if (img?.File?.Length > 0)
                {
                    using var ms = new MemoryStream();
                    await img.File.CopyToAsync(ms);
                    var imageBytes = ms.ToArray();
                    imgTable.Rows.Add(productId, sellerId, imageBytes);
                }
            }

        public async Task<string> CreateAuctionsAsync(Auction auction)
        {
            _context.Auctions.Add(auction);
            await _context.SaveChangesAsync();
            return $"Auction for product {auction.ProductId} created successfully.";
        }


        public async Task<string> UpdateAuctionAsync(int id, Auction updatedAuction)
        {
            var existingAuction = await _context.Auctions.FindAsync(id);
            if (existingAuction != null && !existingAuction.IsDeleted)
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
            if (auction != null && !auction.IsDeleted)
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
                .Where(a => a.SellerId == sellerId && !a.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Auction>> GetAuctionsByProductAsync(int productId)
        {
            return await _context.Auctions
                .Where(a => a.ProductId == productId && !a.IsDeleted)
                .ToListAsync();
        }


        public async Task<string> CloseAuctionAsync(int id, decimal finalBid)
        {
            var auction = await _context.Auctions.FindAsync(id);
            if (auction != null)
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
