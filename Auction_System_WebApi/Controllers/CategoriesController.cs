using Auction_System_Library_Database.Data;
using Auction_System_Library_Infrastructure.Interfaces;
using Auction_System_Library_Database.Models;
using Auction_System_Library_Infrastructure.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Auction_System_Library_Infrastructure.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration.EnvironmentVariables;

namespace Auction_System_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(ICategoryRepository categoryRepository) : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository;

        // GET: api/Categories/Active
        [HttpGet("Active")]
        public async Task<ActionResult<IEnumerable<Category>>> GetActiveCategories()
        {
            var activeCategoryList = await _categoryRepository.GetActiveCategoriesAsync();
            return Ok(activeCategoryList);
        }

        // GET: api/Categories/Active
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
        {
            var activeCategoryList = await _categoryRepository.GetAllCategoriesAsync();
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
