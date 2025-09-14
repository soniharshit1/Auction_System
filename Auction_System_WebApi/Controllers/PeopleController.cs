using Auction_System_Library_Database.Data;
using Auction_System_Library_Infrastructure.DTOs;
using Auction_System_Library_Database.Models;
using Auction_System_Library_Infrastructure.Interfaces;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Auction_System_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController(IPersonRepository personRepository) : ControllerBase
    {
        private readonly IPersonRepository _personRepository = personRepository;



        // GET: api/People
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPeople()
        {
            return Ok(await _personRepository.GetAllPersonsAsync());
        }

        //// GET: api/People/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(int id)
        {
            return Ok(await _personRepository.FindPersonbyIdAsync(id));
        }

        //// PUT: api/People/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePersonDetailsAsync(int id, [FromBody] updatedPersonDTO dto)
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
        public async Task<ActionResult<Person>> PostPerson([FromBody]PersonDTO dto)
        {
            return Ok(await _personRepository.RegisterPersonAsync(dto));
        }

        //// DELETE: api/People/5
        [HttpDelete("{id}")]
       
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
