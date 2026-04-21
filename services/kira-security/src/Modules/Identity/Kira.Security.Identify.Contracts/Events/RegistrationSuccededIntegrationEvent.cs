using MediatR;

namespace Kira.Security.Identify.Core.Events;

public record RegistrationSucceededIntegrationEvent(
    Guid UserId,
    string Email
) : INotification;