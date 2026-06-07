using LoanService.Domain.Entities;

namespace LoanService.Application.Interfaces;

public interface ILoanRepository
{
    Task<Loan?>        GetByIdAsync(Guid loanId);
    Task<List<Loan>>   GetByUserIdAsync(Guid userId);
    Task               AddAsync(Loan loan);
    Task               UpdateAsync(Loan loan);
    Task               SaveChangesAsync();
}
