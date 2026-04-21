using Kira.Security.Core.Options;
using Kira.Security.Core.Services;
using Kira.Security.Core.Abstractions;
using Kira.Security.Management.UseCases.Features.Account.Faults;

using Kira.UseCases.Results;
using Kira.UseCases.Results.Implementations;

using MediatR;
using Microsoft.Extensions.Options;

namespace Kira.Security.Management.UseCases.Features.Account.Commands.ChangePassword;

public sealed class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result<AccountFault>>
{
    private readonly IUserRepository _userRepository;
    private readonly PasswordOptions _passwordOptions;
    
    public ChangePasswordCommandHandler(IUserRepository userRepository, IOptions<PasswordOptions> passwordOptions)
    {
        ArgumentNullException.ThrowIfNull(userRepository, nameof(userRepository));
        ArgumentNullException.ThrowIfNull(passwordOptions, nameof(passwordOptions));
        
        _userRepository = userRepository;
        _passwordOptions = passwordOptions.Value;
    }
    
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task<Result<AccountFault>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(request.UserId, cancellationToken);
        if (user is null) return AccountFault.UserDoesntExists;

        var oldPasswordHash = CryptographyService.HashPassword(request.Model.OldPassword, _passwordOptions.Salt);
        var newPasswordHash = CryptographyService.HashPassword(request.Model.NewPassword, _passwordOptions.Salt);
        
        if (oldPasswordHash != user.PasswordHash) 
            return AccountFault.OldPasswordDoesntMatch;
        
        user.PasswordHash = newPasswordHash;
        await _userRepository.UpdateAsync(user, cancellationToken);

        return ResultMarker.Succeed();
    }
}