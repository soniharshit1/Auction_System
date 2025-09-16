using Auction_System_Library_Database.Data;
using Auction_System_Library_Database.Models;
using Auction_System_Library_Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Auction_System_Library_Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction_System_Library_Infrastructure.Repository
{
    public class TransactionsRepository : ITransactionsRepository
    {
        private readonly AuctionDbContext _context;
        public TransactionsRepository(AuctionDbContext context)
        {
            _context = context;

        }
        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
        {
            var transactions = await _context.Transactions.ToListAsync();

            return transactions;

        }
        public async Task<Transaction?> AddTransactionAsync(TransactionDTO transactionDto)
        {
            var buyerExists = await _context.People.AnyAsync(p => p.UserId == transactionDto.BuyerId);
            if (!buyerExists)
                return null;

            var sellerExists = await _context.People.AnyAsync(p => p.UserId == transactionDto.SellerId);
            if (!sellerExists)
                return null;

            var auctionExists = await _context.Auctions.AnyAsync(p => p.AuctionId == transactionDto.AuctionId);
            if (!auctionExists)
                return null;
            var transaction = new Transaction
            {
                BuyerId = transactionDto.BuyerId,
                AuctionId = transactionDto.AuctionId,
                Amount = transactionDto.Amount,
                PaymentStatus = transactionDto.PaymentStatus,
                PaymentDate = transactionDto.PaymentDate,
                SellerId = transactionDto.SellerId,
                IsDeleted = false
            };
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;

        }
        public async Task<Transaction?> UpdatePaymentStatusAsync(int Id, TransactionDTO transactionDto)
        {
            var transaction = await _context.Transactions.FindAsync(Id);

            if (transaction == null) return null;

            transaction.PaymentStatus = transactionDto.PaymentStatus;

            await _context.SaveChangesAsync();

            return transaction;

        }
        public async Task<IEnumerable<Transaction>> GetTransactionByUserAsync(int UserId)
        {
            var transaction = await _context.Transactions.Where(t => t.BuyerId == UserId || t.SellerId == UserId).ToListAsync();

            return transaction;
        }

    }
}
