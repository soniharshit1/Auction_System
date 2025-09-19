using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auction_System_Library_Database.Models;

namespace Auction_System_Library_Infrastructure.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync();

        Task<Product?> GetProductByIdAsync(int? id);

        Task<Product?> UpdateProductAsync(int id, Product updatedProduct);

        Task<string> AddProductAsync(Product product);

        Task<string> DeleteProductAsync(int id);
    }
}
