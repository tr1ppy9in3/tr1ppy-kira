using System.Diagnostics.CodeAnalysis;
using Kira.UseCases.Results.Exceptions;

namespace Kira.UseCases.Results.Abstractions;

public abstract class BaseResult<TFault>
{
    private protected readonly bool _isSuccess;
    private protected readonly TFault? _fault = default;

    /// <summary>
    /// Gets a value indicating whether the result of the operation was successful.
    /// </summary>
    public bool Success => _isSuccess;

    /// <summary>
    /// Gets a value indicating whether the result of the operation was a failure.
    /// This is the logical inverse of <see cref="Success"/>.
    /// </summary>
    public bool Failure => !_isSuccess;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseResult{TFault}"/> class, representing a successful outcome.
    /// </summary>
    protected BaseResult() => _isSuccess = true;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseResult{TFault}"/> class, representing a failed outcome.
    /// </summary>
    /// <param name="fault">The fault object that describes the reason for the operation's failure.</param>
    protected BaseResult(TFault fault)
    {
        _fault = fault;
        _isSuccess = false;
    }
    /// <summary>
    /// Attempts to retrieve the fault object if the result represents a failure.
    /// </summary>
    /// <param name="fault">
    /// When this method returns, contains the fault object if the result is a failure; 
    /// otherwise, the <see langword="null"/>.
    /// </param>
    /// <returns> 
    /// <see langword="true"/> if the result is a failure and the fault object was successfully retrieved; 
    /// otherwise, <typeparamref name="default(TFault)"/>.
    /// </returns>
    public bool TryGetFault([MaybeNullWhen(false)] out TFault? fault)
    {
        fault = default;

        if (Failure)
        {
            fault = _fault;
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Ensures the operation was successful. If the result represents a failure (<see cref="Failure"/>),
    /// throws an exception created by the provided exception factory, passing in the fault object.
    /// </summary>
    /// <param name="exceptionFactory">A factory that creates an exception based on the fault object.</param>
    /// <exception cref="Exception">Thrown if the result is a <see cref="Failure"/>.</exception>
    public void EnsureSuccessed(Func<TFault, Exception> exceptionFactory) { if (Failure) throw exceptionFactory(_fault!);}

    /// <summary>
    /// Ensures the operation was successful. If the result represents a failure (<see cref="Failure"/>),
    /// throws an exception created by the provided exception factory.
    /// </summary>
    /// <param name="exceptionFactory">A factory that creates an exception.</param>
    /// <exception cref="Exception">Thrown if the result is a <see cref="Failure"/>.</exception>
    public void EnsureSuccessed(Func<Exception> exceptionFactory) => EnsureSuccessed(_ => exceptionFactory());

    /// <summary>
    /// Ensures the operation was successful. If the result represents a failure (<see cref="Failure"/>),
    /// throws the provided exception.
    /// </summary>
    /// <param name="exception">The exception to be thrown if the result is a <see cref="Failure"/>.</param>
    /// <exception cref="Exception">Thrown if the result is a <see cref="Failure"/>.</exception>
    public void EnsureSuccessed(Exception exception) => EnsureSuccessed(() => exception);

    /// <summary>
    /// Ensures the operation was successful. If the result represents a failure (<see cref="Failure"/>),
    /// throws a <see cref="ResultWasFailureException{TFault}"/>,
    /// encapsulating the fault object.
    /// </summary>
    /// <exception cref="ResultWasFailureException{TFault}">Thrown if the result is a <see cref="Failure"/>.</exception>
    public void EnsureSuccessed() => EnsureSuccessed(fault => new ResultWasFailureException<TFault>(fault));

    /// <summary>
    /// Ensures the operation was a failure. If the result represents success (<see cref="Success"/>),
    /// throws an exception created by the provided exception factory.
    /// </summary>
    /// <param name="exceptionFactory">A factory that creates an exception.</param>
    /// <exception cref="Exception">Thrown if the result is a <see cref="Success"/>.</exception>
    public void EnsureFailure(Func<Exception> exceptionFactory) { if (Success) throw exceptionFactory(); }

    /// <summary>
    /// Ensures the operation was a failure. If the result represents success (<see cref="Success"/>),
    /// throws the provided exception.
    /// </summary>
    /// <param name="exception">The exception to be thrown if the result is a <see cref="Success"/>.</param>
    /// <exception cref="Exception">Thrown if the result is a <see cref="Success"/>.</exception>
    public void EnsureFailure(Exception exception) => EnsureFailure(() => exception);

    /// <summary>
    /// Ensures the operation was a failure. If the result represents success (<see cref="Success"/>),
    /// throws a <see cref="ResultWasSuccessException"/>.
    /// </summary>
    /// <exception cref="ResultWasSuccessException">Thrown if the result is a <see cref="Success"/>.</exception>
    public void EnsureFailure() => EnsureFailure(() => new ResultWasSuccessException());

    /// <summary>
    /// Retrieves the fault object if the result represents a failure; otherwise, returns the default value for <typeparamref name="TFault"/>.
    /// This method provides a safe way to access the fault without throwing an exception, making it suitable for scenarios
    /// where you want to handle the absence of a fault explicitly.
    /// </summary>
    /// <returns>The fault object if <see cref="Failure"/>; otherwise, <see langword="default"/> for <typeparamref name="TFault"/> (which may be <see langword="null"/> for reference types).</returns>
    public TFault? GetFaultOrDefault() => _fault;

    /// <summary>
    /// Retrieves the fault object if the result represents a failure.
    /// If the result is a success (<see cref="Success"/>), it throws an exception created by the provided factory.
    /// This method is useful for quickly getting the fault when you expect a failure and want to signal an error
    /// if the operation unexpectedly succeeded.
    /// </summary>
    /// <param name="exceptionFactory">A factory function that creates an <see cref="Exception"/> to be thrown if the result is a success.</param>
    /// <returns>The fault object if the result is a <see cref="Failure"/>.</returns>
    /// <exception cref="Exception">Thrown if the result is a <see cref="Success"/>.</exception>
    public TFault GetFaultOrThrow(Func<Exception> exceptionFactory) => Success ? throw exceptionFactory() : _fault!;

    /// <summary>
    /// Retrieves the fault object if the result represents a failure.
    /// If the result is a success (<see cref="Success"/>), it throws the provided exception.
    /// This overload simplifies throwing a specific exception when a failure is expected but success occurs.
    /// </summary>
    /// <param name="exception">The <see cref="Exception"/> to be thrown if the result is a <see cref="Success"/>.</param>
    /// <returns>The fault object if the result is a <see cref="Failure"/>.</returns>
    /// <exception cref="Exception">Thrown if the result is a <see cref="Success"/>.</exception>
    public TFault GetFaultOrThrow(Exception exception) => GetFaultOrThrow(() => exception);

    /// <summary>
    /// Retrieves the fault object if the result represents a failure.
    /// If the result is a success (<see cref="Success"/>), it throws a <see cref="ResultWasSuccessException"/>.
    /// This is a convenience overload for asserting that the operation should have failed, throwing a default exception on success.
    /// </summary>
    /// <returns>The fault object if the result is a <see cref="Failure"/>.</returns>
    /// <exception cref="ResultWasSuccessException">Thrown if the result is a <see cref="Success"/>.</exception>
    public TFault GetFaultOrThrow() => GetFaultOrThrow(() => new ResultWasSuccessException());
}
