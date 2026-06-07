using LoanService.Application.Interfaces;
using MassTransit;

namespace LoanService.Infrastructure.Messaging;

// Implementación del publisher usando MassTransit
public class MassTransitEventPublisher : IEventPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MassTransitEventPublisher(IPublishEndpoint publishEndpoint)
        => _publishEndpoint = publishEndpoint;

    public async Task PublishAsync<T>(T eventMessage) where T : class
        => await _publishEndpoint.Publish(eventMessage);
}
