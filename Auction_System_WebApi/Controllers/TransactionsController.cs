using Auction_System_Library_Database.Data;
using Auction_System_Library_Database.Models;
using Auction_System_Library_Infrastructure.DTOs;
using Auction_System_Library_Infrastructure.Interfaces;
using Auction_System_Library_Infrastructure.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;

namespace Auction_System_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionsRepository _TransactionsRepository;

        public TransactionsController(ITransactionsRepository TransactionRepository)
        {
            _TransactionsRepository = TransactionRepository;
        }

        // GET: api/Transactions
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            return Ok(await _TransactionsRepository.GetAllTransactionsAsync());
        }
        [HttpPost]
        public async Task<IActionResult> AddTransactionAsync(TransactionDTO transactionDto)
        {
            if (transactionDto == null)
            {
                return BadRequest("Transaction Data is Required");
            }

            var newtransaction = await _TransactionsRepository.AddTransactionAsync(transactionDto);

            if (newtransaction == null)
            {
                return BadRequest("Invalid (Buyer, Seller OR Auction) ID ");
            }

            return Ok(newtransaction);

        }
        [HttpPut]
        [Route("{Id}")]
        [Authorize(Roles ="Admin") ]
        public async Task<IActionResult> UpdatePaymentStatusAsync(int Id, [FromBody] TransactionDTO transactionDto)
        {
            if (transactionDto == null) return BadRequest("Transaction Data is required");

            var update = await _TransactionsRepository.UpdatePaymentStatusAsync(Id, transactionDto);

            if (update == null) return NotFound();
            return Ok(update);
        }
        [HttpGet("user/{UserId}")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetUserTransaction(int UserId)
        {
            return Ok(await _TransactionsRepository.GetTransactionByUserAsync(UserId));
        }
    }
}



