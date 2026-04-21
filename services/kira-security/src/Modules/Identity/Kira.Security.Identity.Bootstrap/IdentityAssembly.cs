using System.Reflection;
using Kira.Security.Identity.UseCases.Features.Registration.Commands.CreateRegistrationState;

namespace Kira.Security.Identity.Bootstrap;

public static class IdentityAssembly
{
    public static Assembly Get => typeof(CreateRegistrationStateCommand).Assembly;
}