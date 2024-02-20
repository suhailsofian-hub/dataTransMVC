using Microsoft.EntityFrameworkCore;
using AuthProject.Models;

namespace AuthProject.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        // public DbSet<Category> Categories { get; set; }
       
        public DbSet<User> Users { get; set; }    
        public DbSet<TransactionModel> Transactions { get; set; }
    
    }
}
