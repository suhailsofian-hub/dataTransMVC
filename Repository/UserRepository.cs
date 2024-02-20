using Microsoft.EntityFrameworkCore;
using AuthProject.Data;
using AuthProject.Interfaces;
using AuthProject.Models;

namespace AuthProject.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Add(User user)
        {
            _context.Add(user);
            return Save();
        }

        public bool Delete(User user)
        {
            _context.Remove(user);
            return Save();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public  IEnumerable<User> GetAll()
        {
            return  _context.Users.ToList();
        }

        public async Task<IEnumerable<User>> GetSliceAsync(int offset, int size)
        {
            return await _context.Users.Skip(offset).Take(size).ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<User?> GetByIdAsyncNoTracking(int id)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

    
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool Update(User club)
        {
            _context.Update(club);
            return Save();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Users.CountAsync();
        }

        public bool exist(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

    }
}