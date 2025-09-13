using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Auction_System_Library_Infrastructure.DTOs;
using Microsoft.EntityFrameworkCore;
using Auction_System_Library_Database.Data;
using Auction_System_Library_Database.Models;
using Auction_System_Library_Infrastructure.Interfaces;

namespace Auction_System_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductRepository productRepository) : ControllerBase
    {
        private readonly IProductRepository _productRepository = productRepository;

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return Ok(await _productRepository.GetProductsAsync());
        }

        // GET: api/Products/id
        [HttpGet("{id}")]
        public async Task<ActionResult<Product?>> GetProduct(int id)
        {
            Product? product = await _productRepository.GetProductByIdAsync(id);

            return product;
        }

        // PUT: api/Products/id
        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> PutProduct(int id, [FromQuery]ProductDTO product)
        {
            var pro = new Product()
            {
                ProductId = id,
                ProductName = product.ProductName,
                CategoryId = product.CategoryId
            };

            Product? updatedProduct = await _productRepository.UpdateProductAsync(id, pro);
            if(updatedProduct == null)
            {
                return NotFound("Product not found");
            }
            return Ok(updatedProduct);
        }

        // POST: api/Products
        [HttpPost]
        public async Task<IActionResult> PostProduct([FromQuery] ProductDTO product)
        {
            var pro = new Product()
            {
                ProductName = product.ProductName,
                CategoryId = product.CategoryId
            };
            string response = await _productRepository.AddProductAsync(pro);
            return Ok(response);
        }

        // DELETE: api/Products/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            string response = await _productRepository.DeleteProductAsync(id);

            if(response == "Product not found")
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
