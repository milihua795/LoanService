using LoanService.Application.Events;
using LoanService.Application.Interfaces;
using LoanService.Domain.Entities;

namespace LoanService.Application.Commands;

public class ReturnLoanHandler
{
    private readonly ILoanRepository  _repository;
    private readonly IEventPublisher  _publisher;

    public ReturnLoanHandler(ILoanRepository repository, IEventPublisher publisher)
    {
        _repository = repository;
        _publisher  = publisher;
    }

    public async Task<Loan> HandleAsync(Guid loanId)
    {
        var loan = await _repository.GetByIdAsync(loanId)
            ?? throw new KeyNotFoundException($"Préstamo {loanId} no encontrado.");

        // Método de dominio que actualiza el estado y la fecha
        loan.RegisterReturn();

        await _repository.UpdateAsync(loan);
        await _repository.SaveChangesAsync();

        // Publicar evento al bus
        await _publisher.PublishAsync(new BookReturnedEvent(
            loan.LoanId,
            loan.BookId,
            loan.ReturnDate!.Value,
            loan.Status == Domain.Enums.LoanStatus.Overdue
        ));

        return loan;
    }
}
