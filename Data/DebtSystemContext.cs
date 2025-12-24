using DebtSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace DebtSystem.Data;

public class DebtSystemContext : DbContext
{
    public DebtSystemContext(DbContextOptions<DebtSystemContext> options)
        : base(options)
    {
    }

    public DbSet<Debtor> Debtors => Set<Debtor>();
    public DbSet<Creditor> Creditors => Set<Creditor>();
    public DbSet<DebtCategory> DebtCategories => Set<DebtCategory>();
    public DbSet<Debt> Debts => Set<Debt>();
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                {
                    property.SetColumnType("timestamp without time zone");
                }
            }
        }

        modelBuilder.Entity<Debt>()
            .HasDiscriminator<string>("DebtType")
            .HasValue<PersonalLoanDebt>("PersonalLoan");

        modelBuilder.Entity<DebtCategory>().HasData(
            new DebtCategory { Id = 1, Name = "Личный заём", Description = "Денежный займ между людьми" },
            new DebtCategory { Id = 2, Name = "Коммунальные услуги", Description = "Оплата ЖКХ" },
            new DebtCategory { Id = 3, Name = "Учёба", Description = "Оплата за обучение" },
            new DebtCategory { Id = 4, Name = "Товары", Description = "Покупка в долг" }
        );

        base.OnModelCreating(modelBuilder);
    }
}