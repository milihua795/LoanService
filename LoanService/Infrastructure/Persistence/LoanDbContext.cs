using LoanService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoanService.Infrastructure.Persistence;

public class LoanDbContext : DbContext
{
    public LoanDbContext(DbContextOptions<LoanDbContext> options) : base(options) { }

    public DbSet<Loan> Loans => Set<Loan>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Loan>(entity =>
        {
            entity.HasKey(l => l.LoanId);
            entity.Property(l => l.BookId).IsRequired();
            entity.Property(l => l.UserId).IsRequired();
            entity.Property(l => l.LoanDate).IsRequired();
            entity.Property(l => l.DueDate).IsRequired();
            entity.Property(l => l.ReturnDate);
            entity.Property(l => l.Status)
                  .HasConversion<string>(); // Guardar como "Active", "Returned", etc.
        });
    }
}
