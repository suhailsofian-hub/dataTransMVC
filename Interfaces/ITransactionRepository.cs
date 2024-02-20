using AuthProject.Models;

namespace AuthProject.Interfaces
{
    public interface ITransactionRepository
    {
        IEnumerable<TransactionModel> GetAll();

         Task<IEnumerable<TransactionModel>> GetAllAsync();

         Task<IEnumerable<TransactionModel>> GetSliceAsync(int offset, int size);

        Task<TransactionModel?> GetByIdAsync(int id);

        bool Add(TransactionModel transaction);

        bool Update(TransactionModel transaction);

        bool Delete(TransactionModel transaction);

        bool Save();

        bool exist(int id);
    }
}