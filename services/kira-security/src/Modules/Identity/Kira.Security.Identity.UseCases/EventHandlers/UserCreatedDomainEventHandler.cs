using Kira.Security.Identify.Core.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Kira.Security.Identity.UseCases.EventHandlers;

public class UserCreatedDomainEventHandler
    : INotificationHandler<UserCreatedDomainEvent>
{
    private readonly IPublisher _publisher;
    private readonly ILogger<UserCreatedDomainEventHandler> _logger;

    public UserCreatedDomainEventHandler(
        IPublisher publisher,
        ILogger<UserCreatedDomainEventHandler> logger
    )
    {
        ArgumentNullException.ThrowIfNull(publisher, nameof(publisher));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        
        _publisher = publisher;
        _logger = logger;
    }
    
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        return _publisher.Publish(new RegistrationSucceededIntegrationEvent(
            notification.UserId, 
            notification.Email
        ), cancellationToken);
    }
}