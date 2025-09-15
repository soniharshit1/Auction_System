//using Auction_System_Library_Infrastructure.Interfaces;
//using Auction_System_Library_Database.Enums;
//using Auction_System_Library_Database.Data;
//using Auction_System_Library_Database.Models;
//using Microsoft.AspNetCore.Mvc;
//using Auction_System_Library_Infrastructure.DTOs;
//using Microsoft.EntityFrameworkCore;
//using Auction_System_Library_Infrastructure.Services;
//using Microsoft.AspNetCore.Identity;

//namespace Auction_System_WebApi.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class AuthController : ControllerBase
//    {
//        private readonly AuctionDbContext _context;
//        private readonly TokenGeneration _authService;

//        public AuthController(AuctionDbContext context, TokenGeneration authService)
//        {
//            _context = context;
//            _authService = authService;
//        }

//        [HttpPost("login")]
//        public async Task<IActionResult> Login(LoginPersonDTO dto)
//        {
//            var user = await _context.People
//                .FirstOrDefaultAsync(u => u.Name == dto.Name);

//            if (user == null) return Unauthorized("Invalid credentials");

//            var passwordHasher = new PasswordHasher<Person>();
//            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

//            if (result == PasswordVerificationResult.Failed)
//                return Unauthorized("Invalid credentials");

//            var token = _authService.GenerateJWT(user);
//            return Ok(new { Token = token });
//        }
//    }
//}
