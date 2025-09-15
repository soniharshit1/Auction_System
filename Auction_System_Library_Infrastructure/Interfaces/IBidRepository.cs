using Auction_System_Library_Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction_System_Library_Infrastucture.Interfaces
{

    public interface IBidRepository
    {
        Task<IEnumerable<Bid>> GetBidsByAuctionAsync(int auctionId);
        Task<IEnumerable<Bid>> GetBidsByBuyerAsync(int buyerId);
        Task<Bid?> GetHighestBidAsync(int auctionId);
        Task<string> PlaceBidAsync(Bid bid);
        Task<Bid?> GetBidByIdAsync(int bidId);
        Task<string> DeleteBidAsync(int bidId); 
    }

}
