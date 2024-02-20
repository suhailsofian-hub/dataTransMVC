using AuthProject.Models;

namespace AuthProject.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();

         IEnumerable<User> GetAll();

        Task<IEnumerable<User>> GetSliceAsync(int offset, int size);
        Task<int> GetCountAsync();

        Task<User?> GetByIdAsync(int id);

        bool Add(User user);

        bool Update(User user);

        bool Delete(User user);

        bool Save();

       bool exist(int id);
    }
}