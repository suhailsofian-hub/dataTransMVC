using Microsoft.EntityFrameworkCore;
using AuthProject.Data;
using AuthProject.Interfaces;
using AuthProject.Models;

namespace AuthProject.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public TransactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Add(TransactionModel transaction)
        {
            _context.Add(transaction);
            return Save();
        }

        public bool Delete(TransactionModel transaction)
        {
            _context.Remove(transaction);
            return Save();
        }

        public async Task<IEnumerable<TransactionModel>> GetAllAsync()
        {
            return await _context.Transactions.ToListAsync();
        }

        public IEnumerable<TransactionModel> GetAll()
        {
            return  _context.Transactions.ToList();
        }


        public async Task<IEnumerable<TransactionModel>> GetSliceAsync(int offset, int size)
        {
            return await _context.Transactions.Skip(offset).Take(size).ToListAsync();
        }

        public async Task<TransactionModel?> GetByIdAsync(int id)
        {
            return await _context.Transactions.FirstOrDefaultAsync(i => i.TransactionId == id);
        }



    
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool Update(TransactionModel transaction)
        {
            _context.Update(transaction);
            return Save();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Transactions.CountAsync();
        }

        public bool exist(int id)
        {
            return _context.Transactions.Any(e => e.TransactionId == id);
        }

    }
}