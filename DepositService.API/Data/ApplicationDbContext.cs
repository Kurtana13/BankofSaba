using DepositService.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace DepositService.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Deposit>(b =>
            {
                b.HasKey(x => x.Id);

                b.Property(d => d.AccountId).IsRequired();
                b.Property(d => d.UserId).IsRequired();
                b.Property(d => d.Name).IsRequired();
            });

           
        }

        public DbSet<Deposit> Deposits { get; set; }
    }
}
