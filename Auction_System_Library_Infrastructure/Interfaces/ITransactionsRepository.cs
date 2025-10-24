using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auction_System_Library_Database.Models;
using Auction_System_Library_Infrastructure.DTOs;

namespace Auction_System_Library_Infrastructure.Interfaces
{
    public interface ITransactionsRepository
    {
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
        Task<Transaction?> AddTransactionAsync(TransactionDTO transactionDto);
        Task<Transaction?> UpdatePaymentStatusAsync(int Id, TransactionDTO transactionDto);
        Task<IEnumerable<Transaction>> GetTransactionByUserAsync(int UserId);
        Task<bool> IsPaymentCompletedAsync(int auctionId);

        // NEW METHOD
        /// <summary>
        /// Retrieves a single Transaction record based on its associated Auction ID.
        /// </summary>
        /// <param name="auctionId">The ID of the auction.</param>
        /// <returns>The Transaction object if found, otherwise null.</returns>
        Task<Transaction?> GetTransactionByAuctionIdAsync(int auctionId);

    }
}