using Auction_System_Library_Database.Data;
using Auction_System_Library_Database.Models;
using Auction_System_Library_Infrastructure.DTOs;
using Auction_System_Library_Infrastructure.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction_System_Library_Infrastructure.Repository
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

        public async Task<string> CreateAuctionWithAttributesAsync(int productId,int sellerId,DateTime startDate,DateTime endDate,decimal startPrice,List<AddAuctionProductAttributesDTO> attributes)
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

            var tvp = new DataTable();
            tvp.Columns.Add("AttributeId", typeof(int));
            tvp.Columns.Add("AttributeValue", typeof(string));

            foreach (var attr in attributes)
            {
                tvp.Rows.Add(attr.AttributeId, attr.AttributeValue);
            }

            var tvpParam = new SqlParameter("@Attributes", tvp)
            {
                SqlDbType = SqlDbType.Structured,
                TypeName = "AuctionAttributeType" 
            };
            command.Parameters.Add(tvpParam);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var message = reader["Message"].ToString();
                var auctionId = reader["AuctionId"].ToString();
                return $"{message} (Auction ID: {auctionId})";
            }

            return "Failed to create auction.";
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
