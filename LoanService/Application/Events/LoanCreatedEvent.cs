namespace LoanService.Application.Events;

// Contrato del evento publicado al bus cuando se registra un préstamo
public record LoanCreatedEvent(
    Guid     LoanId,
    Guid     BookId,
    Guid     UserId,
    DateTime LoanDate,
    DateTime DueDate
);
