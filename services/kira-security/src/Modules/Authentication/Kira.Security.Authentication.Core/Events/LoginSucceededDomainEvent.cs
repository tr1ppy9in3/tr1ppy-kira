using MediatR;

namespace Kira.Security.Authentication.Core.Events;

public record LoginSucceededDomainEvent(
    Guid UserId, 
    DateTime Timestamp,
    LoginType Type, 
    string IpAddress,
    string UserAgent
) : INotification;