using Kira.Security.Authentication.Core.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Kira.Security.Authentication.UseCases.Features.Login.EventsHandlers;

public sealed class LoginSucceededDomainEventHandler : INotificationHandler<LoginSucceededDomainEvent>
{
    private readonly ILogger<LoginSucceededDomainEventHandler> _logger;

    public LoginSucceededDomainEventHandler(ILogger<LoginSucceededDomainEventHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        _logger = logger;
    }
    
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public Task Handle(LoginSucceededDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Received login succeeded domain event!");
        return Task.CompletedTask;
    }
}