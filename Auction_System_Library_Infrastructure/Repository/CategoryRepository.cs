using Auction_System_Library_Database.Data;
using Auction_System_Library_Infrastructure.Interfaces;
using Auction_System_Library_Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auction_System_Library_Database.Enums;

namespace Auction_System_Library_Infrastructure.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AuctionDbContext _context;

        public CategoryRepository(AuctionDbContext context)
        {
            _context = context;
        }
        public async Task<string> AddCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return $"Category {category.CategoryName} added successfully"; ;
        }

        public async Task<string> DeleteCategoryAsync(int id)
        {
            var category = await FindCategoryWithId(id);
            if (category != null) 
            {
                category.IsActive = false;
                _context.SaveChanges();
                return "Category deleted successfully";
            }
            return "Category not found";
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _context.Categories
                .Where(c => c.IsActive == true)
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync(Role role)
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<string> UpdateCategoryAsync(int id, Category updatedCategory)
        {
            var existingCategory = await FindCategoryWithId(id);

            if (existingCategory != null)
            {
                existingCategory.CategoryName = updatedCategory.CategoryName;
                existingCategory.IsActive = updatedCategory.IsActive;
                _context.Categories.Update(existingCategory);
                await _context.SaveChangesAsync();

                return $"Category {existingCategory.CategoryName} is updated successfully";
            }
            return "Category not found";
        }

        private async Task<Category?> FindCategoryWithId(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c=>c.CategoryId == id);
        }
    }
}
