using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Auction_System_Library_Database.Data;
using Auction_System_Library_Database.Models;
using Auction_System_Library_Infrastructure.Interfaces;
using Auction_System_Library_Infrastructure.DTOs;

namespace Auction_System_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionProductImagesController : ControllerBase
    {
        private readonly IAuctionProductImagesRepository _repository;
        public AuctionProductImagesController(IAuctionProductImagesRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("images")]
        public async Task<IActionResult> GetImageUrls([FromQuery] int auctionId, [FromQuery] int productId, [FromQuery] int sellerId)
        {
            var images = await _repository.GetAuctionProductImages(auctionId, productId, sellerId);

            var urls = images
                .Where(img => img.ProductImages != null)
                .Select(img => $"{Request.Scheme}://{Request.Host}/api/auctionproductimages/image/{img.Id}")
                .ToList();

            return Ok(urls);
        }

        [HttpGet("image/{id}")]
        public async Task<IActionResult> GetImage(int id)
        {
            var image = await _repository.GetImageById(id);

            if (image == null || image.ProductImages == null)
            {
                return NotFound(); 
            }

            string contentType = "image/jpeg"; 

            return File(image.ProductImages, contentType);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> PostAuctionProductImage([FromQuery]int auctionId, [FromQuery]int productId, [FromQuery]int sellerId, TestDto imageFile)
        {
            
            var result = await _repository.UploadImage(auctionId, productId, sellerId, imageFile);
            return Ok(result);
        }
    }
}
