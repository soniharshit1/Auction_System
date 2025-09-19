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
            bool isProductAuctioned = await _context.Auctions.AnyAsync(a => a.ProductId == productId && a.IsDeleted == false && a.AuctionId == auctionId);
            if (isProductAuctioned == false)
            {
                throw new ArgumentException($"No product is auctioned with produtcId : {productId} and auctionId : {auctionId}");
            }

            var generalProductAttributes = await _context.GeneralProductAttributes.Where(gpa => gpa.ProductId == productId && gpa.IsDeleted == false).ToListAsync();

            var attributeIds = generalProductAttributes.Select(gpa => gpa.AttributeId).ToList();

            var auctionProductAttributes = await _context.AuctionProductAttributes.Where(apa=> apa.AuctionId ==auctionId && attributeIds.Contains(apa.AttributeId) && apa.IsDeleted == false).ToListAsync();

            var result = generalProductAttributes
                            .GroupJoin(
                                auctionProductAttributes,
                                general => general.AttributeId,
                                auction => auction.AttributeId,
                                (general, auction) => new AuctionProductAttributesDTO
                                {
                                    AttributeId = general.AttributeId,
                                    AttributeName = general.AttributeName,
                                    AttributeValue = auction.OrderByDescending(a => a.Id).FirstOrDefault()?.AttributeValue ?? String.Empty
                                }
                            ).ToList();

            return result;
        }


        public async Task<string> SaveAttributesAsync(int auctionId, int attributeId, string attributeValue)
        {
            var auction = await _context.Auctions.FindAsync(auctionId);
            if(auction == null)
            {
                return "Auction not found.";
            }
            bool isGeneralAttributeDeleted = await _context.GeneralProductAttributes
            .AnyAsync(gpa => gpa.AttributeId == attributeId && gpa.IsDeleted);
            if (isGeneralAttributeDeleted)
            {
                throw new Exception("Attribute not found");
            }

            var attribute = new AuctionProductAttribute
            {
                AuctionId = auctionId,
                AttributeValue = attributeValue,
                AttributeId = attributeId,
                IsDeleted = false
            };
            _context.AuctionProductAttributes.Add(attribute);
            await _context.SaveChangesAsync();

            return "Attributes saved successfully.";
        }
    }
}
