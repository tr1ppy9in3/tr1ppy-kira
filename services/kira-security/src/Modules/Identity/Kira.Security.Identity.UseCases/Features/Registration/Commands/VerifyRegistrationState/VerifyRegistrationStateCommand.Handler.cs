using Kira.Security.Core.Abstractions;
using Kira.Security.Core.Entities;
using Kira.Security.Identity.UseCases.Features.Registration.Faults;
using Kira.Security.UseCases.Abstractions;
using Kira.UseCases.Results;
using Kira.UseCases.Results.Implementations;
using MediatR;

namespace Kira.Security.Identity.UseCases.Features.Registration.Commands.VerifyRegistrationState;

public sealed class VerifyRegistrationStateCommandHandler : IRequestHandler<VerifyRegistrationStateCommand,  Result<RegistrationFault>>
{
    private readonly IUserRepository _userRepository;
    private readonly IRegistrationStateCache _stateCache;

    public VerifyRegistrationStateCommandHandler(
        IUserRepository userRepository,
        IRegistrationStateCache stateCache)
    {
        _userRepository = userRepository;
        _stateCache = stateCache;
    }
    
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task<Result<RegistrationFault>> Handle(VerifyRegistrationStateCommand request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.ToLower();
        
        var state = await _stateCache.GetStateAsync(normalizedEmail, cancellationToken);
        if (state == null) return RegistrationFault.RegistrationTimeExpired;
        
        if (DateTime.UtcNow > state.ExpiredAt)
        {
            await _stateCache.RemoveStateAsync(normalizedEmail, cancellationToken);
            return RegistrationFault.RegistrationTimeExpired;
        }
        
        if (state.VerificationCode != request.VerificationCode)
            return RegistrationFault.InvalidRegistrationCode;
        
        var newUser = new User
        {
            Login = state.Login,
            Email = state.Email,
            PasswordHash = state.PasswordHash,
        };
        
        await _userRepository.AddAsync(newUser, cancellationToken);
        await _stateCache.RemoveStateAsync(normalizedEmail, cancellationToken);
        
        return ResultMarker.Succeed();
    }
}