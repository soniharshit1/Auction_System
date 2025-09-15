using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auction_System_Library_Database.Enums;
using Auction_System_Library_Database.Models;

namespace Auction_System_Library_Infrastructure.Interfaces
{
    public interface ICategoryRepository
    {
        Task<string> UpdateCategoryAsync(int id, Category updatedCategory);
        Task<string> AddCategoryAsync(Category category);
        Task<string> DeleteCategoryAsync(int id);
        Task<IEnumerable<Category>> GetCategoriesAsync(Role role);
    }
}
