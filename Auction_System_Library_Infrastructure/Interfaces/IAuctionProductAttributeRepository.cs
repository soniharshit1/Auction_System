using Auction_System_Library_Database.Models;
using Auction_System_Library_Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction_System_Library_Infrastructure.Interfaces
{
    public interface IAuctionProductAttributeRepository
    {
        // Get all attributes for a specific auction and product
        Task<List<AuctionProductAttributesDTO>> GetAttributesForAuctionAsync(int productId, int auctionId);

        // Save or update attribute values for an auction
        Task<string> SaveAttributesAsync(int auctionId, List<AuctionProductAttributesDTO> attributes);

        // Get a single attribute value by auction and attribute ID
        Task<AuctionProductAttribute?> GetAttributeAsync(int auctionId, int attributeId);

        // Add a new auction-specific attribute
        Task AddAsync(AuctionProductAttribute attribute);

        // Update an existing auction-specific attribute
        Task UpdateAsync(AuctionProductAttribute attribute);

        // Soft delete an attribute (optional)
        Task DeleteAsync(int id);

        // Check if an attribute already exists for an auction
        Task<bool> ExistsAsync(int auctionId, int attributeId);
    }
}
