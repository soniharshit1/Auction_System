using Auction_System_Library_Database.Models;
using Auction_System_Library_Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction_System_Library_Infrastructure.Interfaces
{
    public interface IAuctionRepository
    {
        Task<IEnumerable<Auction>> GetAllAuctionsAsync();
        Task<IEnumerable<Auction>> GetActiveAuctionsAsync();
        Task<IEnumerable<Auction>> GetLiveAuctionsByProductAsync(int productId);
        Task<Auction?> GetAuctionByIdAsync(int id);
        Task<string> CreateAuctionWithAttributesAsync(int productId, int sellerId, DateTime startDate, DateTime endDate, decimal startPrice, List<AddAuctionProductAttributesDTO> attributes, List<TestDto> images);
        Task<string> UpdateAuctionAsync(int id, Auction updatedAuction);
        Task<string> DeleteAuctionAsync(int id);
        Task<IEnumerable<Auction>> GetAuctionsBySellerAsync(int sellerId);
        Task<IEnumerable<Auction>> GetAuctionsByProductAsync(int productId);
        Task<string> CloseAuctionAsync(int id, decimal finalBid);

    }
}
