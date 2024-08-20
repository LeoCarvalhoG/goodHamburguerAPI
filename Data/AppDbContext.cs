using GoodHamburgerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburgerAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Sandwich> Sandwiches { get; set; }
        public DbSet<Extra> Extras { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Order>()
                .Ignore(o => o.ErrorMessage);

            base.OnModelCreating(modelBuilder);
        }
    }
}
