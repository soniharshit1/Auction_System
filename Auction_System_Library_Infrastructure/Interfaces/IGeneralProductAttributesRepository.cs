using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auction_System_Library_Database.Models;
using Auction_System_Library_Infrastructure.DTOs;

namespace Auction_System_Library_Infrastructure.Interfaces
{
    public interface IGeneralProductAttributesRepository
    {
        Task<GeneralProductAttribute?> GetGeneralProductAttributeById(int generalProductAttributeId);
        Task<string> DeleteGeneralProductAttribute(int generalProductAttributeId);
        Task<string> AddGeneralProductAttribute(GeneralProductAttributeDTO generalProductAttributeDTO, int productId);
        Task<string> UpdateGeneralProductAttribute(int generalProductAttributeId,GeneralProductAttributeDTO generalProductAttributeDTO);
        Task<IEnumerable<GeneralProductAttribute>> GetAttributesByProductId(int productId);
        Task<string> RemoveAttributeFromProduct(int productId, int generalProductAttributeId);

    }
}
