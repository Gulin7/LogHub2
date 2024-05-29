using LogHub2.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LogHub2.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
            
        }

        public DbSet<Log> Logs { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
