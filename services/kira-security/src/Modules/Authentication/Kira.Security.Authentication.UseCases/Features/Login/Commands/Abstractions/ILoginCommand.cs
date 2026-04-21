using Kira.Security.Authentication.UseCases.Features.Login.Faults;
using Kira.UseCases.CommandValidation;

namespace Kira.Security.Authentication.UseCases.Features.Login.Commands.Abstractions;

public interface ILoginCommand : IValidatableCommand<LoginFault, LoginResponse>;