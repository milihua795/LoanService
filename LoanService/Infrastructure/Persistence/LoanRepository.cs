using LoanService.Application.Interfaces;
using LoanService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoanService.Infrastructure.Persistence;

public class LoanRepository : ILoanRepository
{
    private readonly LoanDbContext _context;

    public LoanRepository(LoanDbContext context)
        => _context = context;

    public async Task<Loan?> GetByIdAsync(Guid loanId)
        => await _context.Loans.FindAsync(loanId);

    public async Task<List<Loan>> GetByUserIdAsync(Guid userId)
        => await _context.Loans
                         .Where(l => l.UserId == userId)
                         .ToListAsync();

    public async Task AddAsync(Loan loan)
        => await _context.Loans.AddAsync(loan);

    public Task UpdateAsync(Loan loan)
    {
        _context.Loans.Update(loan);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
}
