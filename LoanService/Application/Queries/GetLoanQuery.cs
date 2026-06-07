using LoanService.Application.Interfaces;
using LoanService.Domain.Entities;

namespace LoanService.Application.Queries;

public class GetLoanHandler
{
    private readonly ILoanRepository _repository;

    public GetLoanHandler(ILoanRepository repository)
        => _repository = repository;

    public async Task<Loan> HandleAsync(Guid loanId)
        => await _repository.GetByIdAsync(loanId)
           ?? throw new KeyNotFoundException($"Préstamo {loanId} no encontrado.");

    public async Task<List<Loan>> HandleByUserAsync(Guid userId)
        => await _repository.GetByUserIdAsync(userId);
}
