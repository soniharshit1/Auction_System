using Auction_System_Library.Interfaces;
using Auction_System_Library.Models;
using Auction_System_Library.Data;
using Microsoft.EntityFrameworkCore;

namespace Auction_System_Library.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AuctionDbContext _context;
        public ProductRepository(AuctionDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await FindProductWithId(id);
        }

        public async Task<Product?> UpdateProductAsync(int id, Product updatedProduct)
        {
            var existingProduct = await FindProductWithId(id);

            if (existingProduct != null)
            {
                existingProduct.ProductName = updatedProduct.ProductName;
                existingProduct.CategoryId = updatedProduct.CategoryId;
                _context.Products.Update(existingProduct);
                await _context.SaveChangesAsync();

                return existingProduct;
            }
            return null;
        }

        public async Task<string> AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return $"Product {product.ProductName} added successfully";
        }

        public async Task<string> DeleteProductAsync(int id)
        {
            var product = await FindProductWithId(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return $"Product with productId: {id} is deleted successfully";
            }
            return "Product not found";
        }

        private async Task<Product?> FindProductWithId(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
        }
    }
}
