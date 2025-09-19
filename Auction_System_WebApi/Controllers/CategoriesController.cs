using Auction_System_Library_Infrastructure.Interfaces;
using Auction_System_Library_Database.Enums;
using Auction_System_Library_Database.Models;
using Microsoft.AspNetCore.Mvc;
using Auction_System_Library_Infrastructure.DTOs;
using Microsoft.AspNetCore.Authorization;


namespace Auction_System_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(ICategoryRepository categoryRepository) : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository;

        // GET: api/Categories/Role
        [HttpGet("{role}")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories(Role role)
        {
            var activeCategoryList = await _categoryRepository.GetCategoriesAsync(role);
            return Ok(activeCategoryList);
        }

        // PUT: api/Categories/5
        [HttpPut("{id}")]

        public async Task<IActionResult> PutCategory(int id, CategoryDTO category)
        {
            var cat = new Category()
            {
                CategoryId = id,
                CategoryName = category.CategoryName,
                IsActive = category.IsActive,
            };

            var response = await _categoryRepository.UpdateCategoryAsync(id, cat);
            if(response == "Category not found")
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        // POST: api/Categories
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory([FromQuery]CategoryDTO category)
        {
            var cat = new Category()
            {
                CategoryName = category.CategoryName,
                IsActive = category.IsActive,
            };
            var response = await _categoryRepository.AddCategoryAsync(cat);
            return Ok(response);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            string response = await _categoryRepository.DeleteCategoryAsync(id);

            if (response == "Category not found")
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
