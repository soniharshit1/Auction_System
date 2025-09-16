using Auction_System_Library_Database.Models;
using Auction_System_Library_Infrastructure.DTOs;
using Auction_System_Library_Infrastructure.Interfaces;
using Auction_System_Library_Database.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Auction_System_Library_Infrastructure.Repository
{
    public class AuctionProductAttributeRepository : IAuctionProductAttributeRepository
    {
        private readonly AuctionDbContext _context;
        public AuctionProductAttributeRepository(AuctionDbContext context) 
        {
            _context = context;
        }
        public Task AddAsync(AuctionProductAttribute attribute)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(int auctionId, int attributeId)
        {
            throw new NotImplementedException();
        }

        public Task<AuctionProductAttribute?> GetAttributeAsync(int auctionId, int attributeId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<AuctionProductAttributesDTO>> GetAttributesForAuctionAsync(int productId, int auctionId)
        {
            var generalProductAttributes = await _context.GeneralProductAttributes
                .Where(gpa => gpa.ProductId == productId)
                .ToListAsync();

            var auctionProductAttributes = await _context.AuctionProductAttributes
                .Where(apa => apa.AuctionId == auctionId)
                .ToListAsync();

            var result = generalProductAttributes.Select(generalAttr =>
            {
                var auctionAttr = auctionProductAttributes.FirstOrDefault(a => a.AttributeId == generalAttr.AttributeId);
                if (auctionAttr != null)
                {
                    return new AuctionProductAttributesDTO
                    {
                        AttributeId = generalAttr.AttributeId,
                        AttributeName = generalAttr.AttributeName,
                        AttributeValue = auctionAttr.AttributeValue
                    };
                }
                else
                {
                    return new AuctionProductAttributesDTO
                    {
                        AttributeId = generalAttr.AttributeId,
                        AttributeName = generalAttr.AttributeName,
                        AttributeValue = string.Empty
                    };
                }
            }).ToList();

            return result;
        }

        public async Task<string> SaveAttributesAsync(int auctionId, List<AuctionProductAttributesDTO> attributes)
        {
            var auctionAttributes = attributes.Select(attr => new AuctionProductAttribute
            {
                AuctionId = auctionId,
                AttributeId = attr.AttributeId,
                AttributeValue = attr.AttributeValue
            }).ToList();

            _context.AuctionProductAttributes.AddRange(auctionAttributes);
            await _context.SaveChangesAsync();

            return "Attributes saved successfully.";
        }

        public Task UpdateAsync(AuctionProductAttribute attribute)
        {
            throw new NotImplementedException();
        }
    }
}
