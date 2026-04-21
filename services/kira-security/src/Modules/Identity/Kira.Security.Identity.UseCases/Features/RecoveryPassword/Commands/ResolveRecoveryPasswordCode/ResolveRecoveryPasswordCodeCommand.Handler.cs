using Kira.Security.Core.Abstractions;
using Kira.Security.Core.Options;
using Kira.Security.Core.Services;
using Kira.Security.Identity.UseCases.Features.RecoveryPassword.Faults;
using Kira.Security.UseCases.Abstractions;
using Kira.UseCases.Results;
using Kira.UseCases.Results.Implementations;
using MediatR;
using Microsoft.Extensions.Options;

namespace Kira.Security.Identity.UseCases.Features.RecoveryPassword.Commands.ResolveRecoveryPasswordCode;


public class ResolveRecoveryPasswordCodeCommandHandler
    : IRequestHandler<ResolveRecoveryPasswordCodeCommand, Result<RecoveryPasswordFault>>
{
    
    private readonly ITokenService  _tokenService;
    private readonly PasswordOptions  _passwordOptions;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordRecoveryStateCache _recoveryStateCache;
    
    public ResolveRecoveryPasswordCodeCommandHandler(
        ITokenService tokenService,
        IUserRepository userRepository, 
        IPasswordRecoveryStateCache recoveryStateCache,
        IOptions<PasswordOptions> passwordOptions
    )
    {
        ArgumentNullException.ThrowIfNull(tokenService, nameof(tokenService));
        ArgumentNullException.ThrowIfNull(userRepository, nameof(userRepository));
        ArgumentNullException.ThrowIfNull(recoveryStateCache, nameof(recoveryStateCache));
        ArgumentNullException.ThrowIfNull(passwordOptions, nameof(passwordOptions));
        
        _tokenService = tokenService;
        _userRepository = userRepository;
        _recoveryStateCache = recoveryStateCache;
        _passwordOptions = passwordOptions.Value;
    }
    
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task<Result<RecoveryPasswordFault>> Handle(
        ResolveRecoveryPasswordCodeCommand request, 
        CancellationToken cancellationToken
    )
    {
        var state = await _recoveryStateCache.GetAsync(request.Email, cancellationToken);
        if (state is null || state.Code != request.Code)
            return RecoveryPasswordFault.InvalidOrExpiredCode;
        
        var user = await _userRepository.FindByEmailAsync(request.Email, cancellationToken);
        if (user == null) return RecoveryPasswordFault.InternalError();
        
        user.PasswordHash = CryptographyService.HashPassword(request.NewPassword, _passwordOptions.Salt);
        await _userRepository.UpdateAsync(user, cancellationToken);
        await _recoveryStateCache.RemoveAsync(request.Email, cancellationToken);
        
        await _tokenService.RemoveAllUserTokensAsync(user.Id, cancellationToken);
        return ResultMarker.Succeed();
    }
}