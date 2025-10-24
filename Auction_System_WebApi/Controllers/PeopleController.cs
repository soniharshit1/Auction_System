using Auction_System_Library_Database.Data;
using Auction_System_Library_Database.Enums;
using Auction_System_Library_Database.Models;
using Auction_System_Library_Infrastructure.DTOs;
using Auction_System_Library_Infrastructure.Interfaces;
using Auction_System_Library_Infrastructure.Repository;
using Humanizer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace Auction_System_WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController(IPersonRepository personRepository,IReviewRepository reviewRepository) : ControllerBase
    {
        private readonly IPersonRepository _personRepository = personRepository;
        private readonly IReviewRepository _reviewRepository = reviewRepository;

        // GET: api/People
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Person>>> GetPeople()
        {
            return Ok(await _personRepository.GetAllPersonsAsync());
        }

        //// GET: api/People/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Person>> GetPerson(int id)
        {
            return Ok(await _personRepository.FindPersonbyIdAsync(id));
        }

        /// <summary>
        /// Retrieves the profile and all reviews received by the authenticated user.
        /// </summary>
        /// <returns>A combined PersonProfileDTO object containing person details and reviews.</returns>
        [HttpGet("Me")]
        [Authorize]
        public async Task<ActionResult<PersonProfileDTO>> GetMyProfileWithReviews()
        {
            // 1. Get UserId from the JWT token claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null || !int.TryParse(userIdClaim, out int personId))
            {
                return Unauthorized(new { message = "Invalid or missing user identifier in token." });
            }

            // 2. Fetch Person data
            var person = await _personRepository.FindPersonbyIdAsync(personId);

            if (person == null)
            {
                return NotFound(new { message = $"User profile not found for ID: {personId}." });
            }

            // 3. Fetch Reviews where the authenticated user is the TargetUserId
            // This is the key step: we fetch reviews where TargetUserId == personId
            var receivedReviews = await _reviewRepository.GetReviewsForTargetUserAsync(personId);

            // 4. Map and return a combined DTO
            var profileDto = new PersonProfileDTO
            {
                Name = person.Name,
                Email = person.Email,
                ContactNumber = person.ContactNumber,
                Role = person.Role, // Assuming 'person' object has the Role property
                PersonId = personId,

                // Assign the fetched reviews to the ReceivedReviews property
                SellerReviews = receivedReviews

               
            };

            return Ok(profileDto);
        }
        //// PUT: api/People/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "User,Agent,Admin")]
        public async Task<IActionResult> UpdatePersonDetailsAsync(int id, [FromBody] UpdatedPersonDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid request payload.");
            }

            var updatedPerson = await _personRepository.UpdatePersonDetailsAsync(id, dto);

            if (updatedPerson == null)
            {
                return NotFound($"Person with ID {id} not found.");
            }

            // Optional: return a sanitized DTO instead of the full entity
            var response = new
            {
                updatedPerson.UserId,
                updatedPerson.Email,
                updatedPerson.ContactNumber
            };

            return Ok(response);
        }


        // POST: api/People
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<Person>> PostPerson([FromBody]PersonDTO dto)
        {
            return Ok(await _personRepository.RegisterPersonAsync(dto));
        }

        //// DELETE: api/People/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var result = await _personRepository.DeletePersonAsync(id);
            if (result == null)
            {
                return NotFound($"Person with ID {id} not found.");
            }

            return Ok(result);
        }
    }
}
