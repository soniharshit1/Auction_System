using Auction_System_Library_Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Auction_System_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public MailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] string recipient)
        {
            await _emailService.SendSimpleEmailAsync(recipient, "Test from Auction System", "Hello — this is a test email.");
            return Ok("Email sent");
        }
    }
}
