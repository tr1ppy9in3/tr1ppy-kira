namespace Kira.UseCases.Results.Exceptions;

public sealed class ResultWasFailureException<TFault>(TFault fault, string? message = default) : ResultWasFailureException(message)
{
    public TFault Fault { get; init; } = fault;
}
