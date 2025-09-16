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
        
        public async Task<string> DeleteAsync(int id)
        {
            var attribute = await _context.AuctionProductAttributes.FindAsync(id);
            if (attribute != null)
            {
                attribute.IsDeleted = true;
                _context.AuctionProductAttributes.Update(attribute);
                await _context.SaveChangesAsync();
                return "Attribute deleted successfully.";
            }
            else
            {
                return "Attribute not found or has been deleted.";
            }
        }

        public async Task<List<AuctionProductAttributesDTO>> GetAttributesForAuctionAsync(int productId, int auctionId)
        {
            var generalProductAttributes = await _context.GeneralProductAttributes
                .Where(gpa => gpa.ProductId == productId)
                .Where(gpa => gpa.IsDeleted == false)
                .ToListAsync();

            var auctionProductAttributes = await _context.AuctionProductAttributes
                .Where(apa => apa.AuctionId == auctionId)
                .Where(apa => apa.IsDeleted == false)
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

        public async Task<string> SaveAttributesAsync(int auctionId, List<AddAuctionProductAttributesDTO> attributes)
        {
            var auction = await _context.Auctions.FindAsync(auctionId);
            if(auction == null)
            {
                return "Auction not found.";
            }
            var auctionAttributes = attributes.Select(attr => new AuctionProductAttribute
            {
                AuctionId = auctionId,
                AttributeValue = attr.AttributeValue
            }).ToList();

            _context.AuctionProductAttributes.AddRange(auctionAttributes);
            await _context.SaveChangesAsync();

            return "Attributes saved successfully.";
        }

        public async Task<string> UpdateAsync(int id, AuctionProductAttributesDTO attribute)
        {
            var existingAttribute = await _context.AuctionProductAttributes
                .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);

            if (existingAttribute == null)
            {
                return "Attribute not found or has been deleted.";
            }

            existingAttribute.AttributeValue = attribute.AttributeValue;

            _context.AuctionProductAttributes.Update(existingAttribute);
            await _context.SaveChangesAsync();

            return "Attribute updated successfully.";
        }

    }
}
