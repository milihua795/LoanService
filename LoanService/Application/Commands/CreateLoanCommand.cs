using LoanService.Application.Events;
using LoanService.Application.Interfaces;
using LoanService.Domain.Entities;

namespace LoanService.Application.Commands;

public record CreateLoanRequest(Guid BookId, Guid UserId, int DurationDays = 14);

public class CreateLoanHandler
{
    private readonly ILoanRepository  _repository;
    private readonly IEventPublisher  _publisher;

    public CreateLoanHandler(ILoanRepository repository, IEventPublisher publisher)
    {
        _repository = repository;
        _publisher  = publisher;
    }

    public async Task<Loan> HandleAsync(CreateLoanRequest request)
    {
        // Crear el préstamo usando el factory method del dominio
        var loan = Loan.Create(request.BookId, request.UserId, request.DurationDays);

        await _repository.AddAsync(loan);
        await _repository.SaveChangesAsync();

        // Publicar evento al bus de forma asíncrona
        await _publisher.PublishAsync(new LoanCreatedEvent(
            loan.LoanId,
            loan.BookId,
            loan.UserId,
            loan.LoanDate,
            loan.DueDate
        ));

        return loan;
    }
}
