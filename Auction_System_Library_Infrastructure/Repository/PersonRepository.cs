using Auction_System_Library_Infrastructure.Interfaces;
using Auction_System_Library_Database.Models;
using Auction_System_Library_Infrastructure.DTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auction_System_Library_Database.Data;
using Microsoft.EntityFrameworkCore;

namespace Auction_System_Library_Infrastructure.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private readonly AuctionDbContext _context;
        public PersonRepository(AuctionDbContext context) 
        {
            _context = context;
        }
        public async Task<string?> DeletePersonAsync(int userOrAgentId)
        {
            var person = await _context.People.FindAsync(userOrAgentId);
            if (person == null)
            {
                return null;
            }

            person.IsDeleted = true;

            //// Soft delete related entities
            //var auctions = _context.Auctions.Where(a => a.SellerId == userOrAgentId);
            //foreach (var auction in auctions)
            //{
            //    auction.IsDeleted = true;
            //}

            //var bids = _context.Bids.Where(b => b.BuyerId == userOrAgentId);
            //foreach (var bid in bids)
            //{
            //    bid.IsDeleted = true;
            //}

            //var transactions = _context.Transactions.Where(t => t.BuyerId == userOrAgentId || t.SellerId == userOrAgentId);
            //foreach (var transaction in transactions)
            //{
            //    transaction.IsDeleted = true;
            //}

            //var reviews = _context.Reviews.Where(r => r.UserId == userOrAgentId || r.TargetUserId == userOrAgentId);
            //foreach (var review in reviews)
            //{
            //    review.IsDeleted = true;
            //}

            //var approvals = _context.Approvals.Where(a => a.AgentId == userOrAgentId);
            //foreach (var approval in approvals)
            //{
            //    approval.IsDeleted = true;
            //}

            //var images = _context.AuctionProductImages.Where(img => img.SellerId == userOrAgentId);
            //foreach (var image in images)
            //{
            //    image.IsDeleted = true;
            //}

            await _context.SaveChangesAsync();
            return $"Person with ID {userOrAgentId} and related records soft deleted.";
        }


        public async Task<Person?> FindPersonbyIdAsync(int userOrAgentId)
        {
            var person = await _context.People.FindAsync(userOrAgentId);
            if (person is null)
            {
                return null;
            }

            return person;
        }

        public async Task<IEnumerable<Person>> GetAllPersonsAsync()
        {
            return await _context.People.ToListAsync();
        }

        public async Task<Person> RegisterPersonAsync(PersonDTO dto)
        {
            if (dto.Role is not Auction_System_Library_Database.Enums.Role.User && dto.Role is not Auction_System_Library_Database.Enums.Role.Agent)
            {
                throw new ArgumentException("Invalid Role. Only User or Agent Allowed");
            }
            if (await _context.People.AnyAsync(p => p.Name == dto.Name))
            {
                throw new ArgumentException("name already exists");
            }
            if (await _context.People.AnyAsync(p => p.Email == dto.Email))
            {
                throw new ArgumentException("Email already exists");
            }
            var person = new Person
            {
                Name = dto.Name,
                Email = dto.Email,
                Role = dto.Role,
                ContactNumber = dto.ContactNumber
            };
            person.PasswordHash = new PasswordHasher<Person>()
                                  .HashPassword(person, dto.Password);

            _context.People.Add(person);
            await _context.SaveChangesAsync();
            return person;
        }

        //can only update email and contact number
        public async Task<Person?> UpdatePersonDetailsAsync(int id, updatedPersonDTO dto)
        {
            var person = await _context.People.FindAsync(id);
            if (person is null)
            {
                return null;
            }
            
            person.Email = dto.Email;
            person.ContactNumber = dto.ContactNumber;

            _context.People.Update(person);
            await _context.SaveChangesAsync();
            return person;
        }

        public Task<Person?> UpdatePersonDetailsAsync(int id, UpdatedPersonDTO person)
        {
            throw new NotImplementedException();
        }
    }
}
