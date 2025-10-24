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
            var transactions = await _context.Transactions.Where(t => t.IsDeleted == false).ToListAsync();

            return transactions;

        }
        public async Task<Transaction?> AddTransactionAsync(TransactionDTO transactionDto)
        {
            var buyerExists = await _context.People.Where(p => p.IsDeleted == false).AnyAsync(p => p.UserId == transactionDto.BuyerId);
            if (!buyerExists)
                return null;

            var sellerExists = await _context.People.Where(p => p.IsDeleted == false).AnyAsync(p => p.UserId == transactionDto.SellerId);
            if (!sellerExists)
                return null;

            var auctionExists = await _context.Auctions.Where(p => p.IsDeleted == false).AnyAsync(p => p.AuctionId == transactionDto.AuctionId);
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
        //public async Task<Transaction?> UpdatePaymentStatusAsync(int Id, TransactionDTO transactionDto)
        //{
        //    var transaction = await _context.Transactions.FirstOrDefaultAsync(p => p.TransactionId == Id && p.IsDeleted == false);

        //    if (transaction == null) return null;

        //    transaction.PaymentStatus = transactionDto.PaymentStatus;

        //    await _context.SaveChangesAsync();

        //    return transaction;

        //}

        // Auction_System_Library_Infrastructure.Repository/TransactionsRepository.cs

public async Task<Transaction?> UpdatePaymentStatusAsync(int Id, TransactionDTO transactionDto)
{
    var transaction = await _context.Transactions.FirstOrDefaultAsync(p => p.TransactionId == Id && p.IsDeleted == false);

    if (transaction == null) return null;

    // 1. Update the Payment Status
    transaction.PaymentStatus = transactionDto.PaymentStatus;

    // 2. CRITICAL CHANGE: Set PaymentDate if the status is being set to TRUE (Paid/1)
    if (transactionDto.PaymentStatus == true)
    {
        // Set PaymentDate to now when the admin confirms payment
        transaction.PaymentDate = DateTime.UtcNow; 
    }
    else if (transactionDto.PaymentStatus == false)
    {
        // If status is set back to false (e.g., pending/0), clear the payment date
        transaction.PaymentDate = null; 
    }

    _context.Transactions.Update(transaction); // Explicit update (optional but safe)

    await _context.SaveChangesAsync();

    return transaction;

}

       
        public async Task<IEnumerable<Transaction>> GetTransactionByUserAsync(int UserId)
        {
            var transaction = await _context.Transactions
                 .Where(p => !p.IsDeleted && (p.BuyerId == UserId || p.SellerId == UserId))
                 .ToListAsync();

            return transaction;
        }

        public async Task<bool> IsPaymentCompletedAsync(int auctionId)
        {
            // Step 1: Search for the transaction with the given auctionId that is not deleted
            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(t => !t.IsDeleted && t.AuctionId == auctionId);

            // Step 2: If no transaction is found, return false
            if (transaction == null)
            {
                return false;
            }

            // Step 3: Check if the payment status is completed (1)
            return transaction.PaymentStatus == true;
        }

        // Inside your Repositories/TransactionsRepository.cs

        public async Task<Transaction?> GetTransactionByAuctionIdAsync(int auctionId)
        {
            // Use FindAsync or FirstOrDefaultAsync based on what is indexed.
            // Since AuctionId is likely not the primary key (TransactionId is), 
            // we use FirstOrDefaultAsync.

            var transaction = await _context.Transactions
                .Where(t => t.AuctionId == auctionId)
                .FirstOrDefaultAsync();

            return transaction;
        }

    }
}