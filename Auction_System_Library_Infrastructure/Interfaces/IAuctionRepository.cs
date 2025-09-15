using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auction_System_Library_Database.Models;

namespace Auction_System_Library_Infrastructure.Interface
{
    public interface IAuctionRepository
    {
        Task<IEnumerable<Auction>> GetAllAuctionsAsync();
        Task<IEnumerable<Auction>> GetActiveAuctionsAsync();
        Task<Auction?> GetAuctionByIdAsync(int id);
        Task<string> CreateAuctionsAsync(Auction auction);
        Task<string> UpdateAuctionAsync(int id, Auction updatedAuction);
        Task<string> DeleteAuctionAsync(int id);
        Task<IEnumerable<Auction>> GetAuctionsBySellerAsync(int sellerId);
        Task<IEnumerable<Auction>> GetAuctionsByProductAsync(int productId);
        Task<string> CloseAuctionAsync(int id, decimal finalBid);

    }
}
