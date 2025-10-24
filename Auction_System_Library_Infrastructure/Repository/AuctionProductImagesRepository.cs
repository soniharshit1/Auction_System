using Auction_System_Library_Database.Data;
using Auction_System_Library_Database.Models;
using Auction_System_Library_Infrastructure.DTOs;
using Auction_System_Library_Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Auction_System_Library_Infrastructure.Repository
{
    public class AuctionProductImagesRepository : IAuctionProductImagesRepository
    {
        private readonly AuctionDbContext _context;

        public AuctionProductImagesRepository(AuctionDbContext context) 
        {
            _context = context;
        }

        public async Task<List<AuctionProductImage>> GetAuctionProductImages(int auctionId, int productId, int sellerId)
        {
            return await _context.AuctionProductImages
                .Where(img => img.AuctionId == auctionId && img.ProductId == productId && img.SellerId == sellerId && !img.IsDeleted)
                .ToListAsync();
        }

        public async Task<AuctionProductImage?> GetImageById(int id)
        {
            return await _context.AuctionProductImages
                .Where(img => img.Id == id && !img.IsDeleted)
                .Select(img => new AuctionProductImage
                {
                    Id = img.Id,
                    ProductImages = img.ProductImages
                })
                .FirstOrDefaultAsync();
        }

    }
}
