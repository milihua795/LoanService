namespace LoanService.Application.Events;

// Contrato del evento publicado al bus cuando se devuelve un libro
public record BookReturnedEvent(
    Guid     LoanId,
    Guid     BookId,
    DateTime ReturnDate,
    bool     WasOverdue
);
