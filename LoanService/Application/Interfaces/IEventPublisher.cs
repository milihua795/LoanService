namespace LoanService.Application.Interfaces;

public interface IEventPublisher
{
    Task PublishAsync<T>(T eventMessage) where T : class;
}
