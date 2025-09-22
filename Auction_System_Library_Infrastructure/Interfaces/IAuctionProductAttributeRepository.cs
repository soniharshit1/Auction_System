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
        Task<List<AuctionProductAttributesDTO>> GetAttributesForAuctionAsync(int productId, int auctionId);

        Task<string> SaveAttributesAsync(int auctionId, int attributeId, string attributeValue);

        Task<string> DeleteAsync(int id);
    }
}
