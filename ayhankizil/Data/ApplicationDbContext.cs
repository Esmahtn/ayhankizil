using Microsoft.EntityFrameworkCore;
using ayhankizil.Models;

namespace ayhankizil.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Paylasim> Paylasimlar { get; set; }
        // Data/ApplicationDbContext.cs
        public DbSet<Admin> Admins { get; set; }
    }
}
