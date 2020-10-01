using EscrowAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EscrowAPI.Services
{
    public interface ITransactionService
    {
        Task<List<Transaction>> GetTransactionsAsync();

        Task<Transaction> GetTransactionByIdAsync(Guid transactionId);
        Task<bool> CreateTransactionAsync(Transaction transaction);

        Task<bool> UpdateTransactionAsync(Transaction transactionToUpdate);

        Task<bool> DeleteTransactionAsync(Guid transactionId);
        Task<bool> UserOwnsTransactionAsync(Guid transactionId, string userId);
        Task<bool> UserBuyingTransactionAsync(Guid transactionId, string userEmail);
    }
}
