using EscrowAPI.Data;
using EscrowAPI.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EscrowAPI.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly DataContext _dataContext;

        public TransactionService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> CreateTransactionAsync(Transaction transaction)
        {
           await _dataContext.Transactions.AddAsync(transaction);
            var created = await _dataContext.SaveChangesAsync();
            return created > 0;
        }
        public async Task<bool> DeleteTransactionAsync(Guid transactionId)
        {
            var transaction = await GetTransactionByIdAsync(transactionId);
            if (transaction == null)
                return false;
            _dataContext.Transactions.Remove(transaction);
            var deleted = await _dataContext.SaveChangesAsync();

            return deleted > 0;
        }

        public async Task<Transaction> GetTransactionByIdAsync(Guid transactionId)
        {
            return await _dataContext.Transactions.SingleOrDefaultAsync(x => x.Id == transactionId);
        }

        public async Task<List<Transaction>> GetTransactionsAsync()
        {
            return await _dataContext.Transactions.ToListAsync();
        }

        public async Task<bool> UpdateTransactionAsync(Transaction transactionToUpdate)
        {
            _dataContext.Transactions.Update(transactionToUpdate);
            var updated = await _dataContext.SaveChangesAsync();
            return updated > 0;
        }

        public async Task<bool> UserOwnsTransactionAsync(Guid transactionId, string userId)
        {
            var transaction = await _dataContext.Transactions.AsNoTracking().SingleOrDefaultAsync(x => x.Id == transactionId);
            if (transaction == null)
            {
                return false;
            }
            if (transaction.UserId != userId)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> UserBuyingTransactionAsync(Guid transactionId, string userEmail)
        {
            var transaction = await _dataContext.Transactions.AsNoTracking().SingleOrDefaultAsync(x => x.Id == transactionId);
            if (transaction == null)
            {
                return false;
            }
            if (transaction.Email != userEmail)
            {
                return false;
            }
            return true;
        }

    }
}
