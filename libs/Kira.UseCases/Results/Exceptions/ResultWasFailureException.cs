namespace Kira.UseCases.Results.Exceptions;

public class ResultWasFailureException(string? message = default) : Exception(message);