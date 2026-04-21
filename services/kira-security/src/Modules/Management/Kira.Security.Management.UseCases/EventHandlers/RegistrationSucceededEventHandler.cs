using Kira.Security.Identify.Core.Events;
using Kira.Security.Management.Core;
using Kira.Security.Management.Core.Abstractions;
using Kira.Security.Management.Core.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Kira.Security.Management.UseCases.EventHandlers;

public sealed class RegistrationSucceededEventHandler : INotificationHandler<RegistrationSucceededIntegrationEvent>
{
    private readonly ILogger<RegistrationSucceededEventHandler> _logger;
    private readonly IUserProfileRepository  _userProfileRepository;

    public RegistrationSucceededEventHandler(
        ILogger<RegistrationSucceededEventHandler> logger,
        IUserProfileRepository  userProfileRepository
    )
    {
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        ArgumentNullException.ThrowIfNull(userProfileRepository, nameof(userProfileRepository));
        
        _logger = logger;
        _userProfileRepository = userProfileRepository;
    }
    
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task Handle(RegistrationSucceededIntegrationEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Received RegistrationSucceededIntegrationEvent in management!");
        await _userProfileRepository.AddAsync(new UserProfile()
        {
            UserId = notification.UserId,
        });
    }
}