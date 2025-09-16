using Auction_System_Library_Database.Data;
using Auction_System_Library_Database.Models;
using Auction_System_Library_Infrastructure.DTOs;
using Auction_System_Library_Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction_System_Library_Infrastructure.Repository
{
    public class GeneralProductAttributesRepository : IGeneralProductAttributesRepository
    {
        private readonly AuctionDbContext _context;

        public GeneralProductAttributesRepository(AuctionDbContext context)
        {
            _context = context;
        }

        public async Task<string> AddGeneralProductAttribute(GeneralProductAttributeDTO generalProductAttributeDTO , int productId)
        {

            var newAttribute = new GeneralProductAttribute
            {
                AttributeName = generalProductAttributeDTO.AttributeName,
                ProductId = productId,
                IsDeleted = false
            };

            _context.GeneralProductAttributes.Add(newAttribute);
            await _context.SaveChangesAsync();

            return "Attribute added successfully.";

        }

        public async Task<string> DeleteGeneralProductAttribute(int generalProductAttributeId)
        {
            var attribute = await FindGeneralProductAttributeByIdASync(generalProductAttributeId);
            if(attribute == null)
            {
                return $"Attribute with id: {generalProductAttributeId} not found.";
            }   
            _context.GeneralProductAttributes.Remove(attribute);
            await _context.SaveChangesAsync();
            return $"Attribute with id: {generalProductAttributeId} is deleted successfully";
        }

        public async Task<IEnumerable<GeneralProductAttribute>> GetAttributesByProductId(int productId)
        {
            return await _context.GeneralProductAttributes
                .Where(gp => gp.IsDeleted == false && gp.ProductId == productId)
                .ToListAsync();
        }

        public async Task<GeneralProductAttribute?> GetGeneralProductAttributeById(int generalProductAttributeId)
        {
            return await FindGeneralProductAttributeByIdASync(generalProductAttributeId);
        }

        public async Task<string> RemoveAttributeFromProduct(int productId, int generalProductAttributeId)
        {
            var attribute = await FindGeneralProductAttributeByIdASync(generalProductAttributeId);

            if (attribute == null || attribute.ProductId != productId)
            {
                return $"Attribute with ID {generalProductAttributeId} not found for Product ID {productId}.";
            }

            attribute.IsDeleted = true;
            await _context.SaveChangesAsync();

            return $"Attribute with ID {generalProductAttributeId} removed from Product ID {productId}.";
        }

        public async Task<string> UpdateGeneralProductAttribute(int generalProductAttributeId, GeneralProductAttributeDTO generalProductAttributeDTO)
        {
            var attribute = await FindGeneralProductAttributeByIdASync(generalProductAttributeId);
            if (attribute == null)
            {
                return $"Attribute with id: {generalProductAttributeId} not found.";
            }
            attribute.AttributeName = generalProductAttributeDTO.AttributeName;
            _context.GeneralProductAttributes.Update(attribute);
            await _context.SaveChangesAsync();
            return $"Attribute with id: {generalProductAttributeId} is updated successfully";
        }

        public async Task<GeneralProductAttribute?> FindGeneralProductAttributeByIdASync(int generalProductAttributeId)
        {
            return await _context.GeneralProductAttributes.Where(gp => gp.IsDeleted == false).
                FirstOrDefaultAsync(gp => gp.AttributeId == generalProductAttributeId);
        }
    }
}
