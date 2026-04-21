using MediatR;

namespace Kira.Security.Identify.Core.Events;

public record UserCreatedDomainEvent(Guid UserId, string Email) : INotification;