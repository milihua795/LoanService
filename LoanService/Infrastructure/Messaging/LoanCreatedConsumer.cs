using LoanService.Application.Events;
using MassTransit;

namespace LoanService.Infrastructure.Messaging;

// Consumidor del evento LoanCreated
// En producción estaría en BookService; aquí lo simulamos en el mismo proyecto
public class LoanCreatedConsumer : IConsumer<LoanCreatedEvent>
{
    private readonly ILogger<LoanCreatedConsumer> _logger;

    public LoanCreatedConsumer(ILogger<LoanCreatedConsumer> logger)
        => _logger = logger;

    public Task Consume(ConsumeContext<LoanCreatedEvent> context)
    {
        var ev = context.Message;
        // Aquí BookService descontaría el stock del libro
        _logger.LogInformation(
            "[EVENTO RECIBIDO] LoanCreated → BookId: {BookId} | UserId: {UserId} | Vence: {DueDate}",
            ev.BookId, ev.UserId, ev.DueDate);

        return Task.CompletedTask;
    }
}

// Consumidor del evento BookReturned
public class BookReturnedConsumer : IConsumer<BookReturnedEvent>
{
    private readonly ILogger<BookReturnedConsumer> _logger;

    public BookReturnedConsumer(ILogger<BookReturnedConsumer> logger)
        => _logger = logger;

    public Task Consume(ConsumeContext<BookReturnedEvent> context)
    {
        var ev = context.Message;
        // Aquí BookService restauraría el stock del libro
        _logger.LogInformation(
            "[EVENTO RECIBIDO] BookReturned → BookId: {BookId} | Devuelto: {ReturnDate} | Tardío: {WasOverdue}",
            ev.BookId, ev.ReturnDate, ev.WasOverdue);

        return Task.CompletedTask;
    }
}
