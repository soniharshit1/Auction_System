using Auction_System_Library_Database.Models;
using Auction_System_Library_Infrastructure.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction_System_Library_Infrastructure.Interfaces
{
    public interface IAuctionProductImagesRepository
    {
        Task<AuctionProductImage?> GetImageById(int id);
    }
}
