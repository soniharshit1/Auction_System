using Auction_System_Library_Database.Data;
using Auction_System_Library_Database.Models;
using Auction_System_Library_Infrastructure.DTOs;
using Auction_System_Library_Infrastructure.Interfaces;
using Microsoft.Data.SqlClient;
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
        private readonly IEmailService _emailService;
        private readonly IApprovalsRepository _approvalsRepository;

        public AuctionRepository(AuctionDbContext context, IEmailService emailService, IApprovalsRepository approvalsRepository)
        {
            _context = context;
            _emailService = emailService;
            _approvalsRepository = approvalsRepository;
        }

        public async Task<IEnumerable<Auction>> GetAllAuctionsAsync()
        {
            return await _context.Auctions
                .Where(a => !a.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Auction>> GetActiveAuctionsAsync()
        {
            return await _context.Auctions
                .Where(a => a.Status == true && a.EndDate > DateTime.Now && !a.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Auction>> GetLiveAuctionsByProductAsync(int productId)
        {
            return await _context.Auctions
                .Where(a => a.Status == true && a.EndDate > DateTime.Now && a.ProductId == productId && !a.IsDeleted)
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

            var imgParam = new SqlParameter("@Images", imgTable)
            {
                SqlDbType = SqlDbType.Structured,
                TypeName = "AuctionImageType"
            };
            command.Parameters.Add(imgParam);

            int auctionId = 0;

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var message = reader["Message"].ToString();
                auctionId = (int)reader["AuctionId"];
            }

            await connection.CloseAsync();
            var response = await _approvalsRepository.AddApprovalAsync(auctionId);
            
            return response;
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
            if (auction != null && !auction.IsDeleted)
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
