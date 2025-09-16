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
    }
}
