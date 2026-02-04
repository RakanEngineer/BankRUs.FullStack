using BankRUs.Domain.Entities;
using BankRUs.Intrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BankRUs.Intrastructure.Persistance;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<BankAccount>(builder =>
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Balance)
              .HasPrecision(18, 2);

            builder
                .HasIndex(b => b.AccountNumber)
                .IsUnique();

            builder.HasMany(x => x.Transactions)
             .WithOne(t => t.BankAccount)
             .HasForeignKey(t => t.BankAccountId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<BankAccount>().
            HasOne<ApplicationUser>().
            WithMany().
            HasForeignKey(b => b.UserId);

        builder.Entity<Transaction>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
            .ValueGeneratedNever();

            entity.Property(x => x.Amount)
                .HasPrecision(18, 2);

            entity.Property(x => x.BalanceAfter)
                .HasPrecision(18, 2);

            entity.Property(x => x.Type)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(x => x.Currency)
                .IsRequired()
                .HasMaxLength(3);

            entity.Property(x => x.Reference)
                .HasMaxLength(140);

            entity.Property(x => x.CreatedAt)
                .IsRequired();

            entity.HasIndex(x => x.CreatedAt);
        });
    }

    public DbSet<BankAccount> BankAccounts => Set<BankAccount>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

}

