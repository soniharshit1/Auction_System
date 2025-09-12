using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auction_System_Library_Database.Models;

namespace Auction_System_Library.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetActiveCategoriesAsync();
        Task<string> UpdateCategoryAsync(int id, Category updatedCategory);
        Task<string> AddCategoryAsync(Category category);
        Task<string> DeleteCategoryAsync(int id);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
    }
}
