using Microsoft.EntityFrameworkCore;
using BankofSaba.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace BankofSaba.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<User,IdentityRole,string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(b =>
            {
                b.HasKey(x => x.Id);

                b.HasMany(c => c.Accounts)
                .WithOne(a => a.User)
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Account>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Balance)
                 .HasPrecision(18, 2); // Precision: 18, Scale: 2
                                       // left         , right
            });


            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "user", NormalizedName = "USER" }
            );

        }
        public DbSet<Account> Accounts { get; set; }



    }
}
