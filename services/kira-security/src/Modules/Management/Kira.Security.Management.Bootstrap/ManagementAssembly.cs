using System.Reflection;
using Kira.Security.Management.UseCases.EventHandlers;

namespace Kira.Security.Management.Bootstrap;

public class ManagementAssembly
{
    public static Assembly Get => typeof(RegistrationSucceededEventHandler).Assembly;
}