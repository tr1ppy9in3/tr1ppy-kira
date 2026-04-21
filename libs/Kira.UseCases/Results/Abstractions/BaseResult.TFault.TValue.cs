using System.Diagnostics.CodeAnalysis;
using Kira.UseCases.Results.Exceptions;

namespace Kira.UseCases.Results.Abstractions;

public abstract class BaseResult<TFault, TValue> : BaseResult<TFault>
{
    private protected readonly TValue? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResultMarker"/> class representing a failed operation.
    /// </summary>
    /// <param name="fault">The fault object indicating the reason for failure.</param>
    public BaseResult(TFault fault) : base(fault) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResultMarker"/> class representing a succesfull operation.
    /// </summary>
    /// <param name="value">The success value.</param>
    public BaseResult(TValue value) : base() => _value = value;

    /// <summary>
    /// Attempts to retrieve the success value or the fault object from the result.
    /// This method is the primary way to safely extract encapsulated data from the result type.
    /// </summary>
    /// <param name="value">
    /// When this method returns <see langword="true"/>, contains the success value; 
    /// otherwise, the default value of <typeparamref name="TValue"/> (which may be <see langword="null"/> for reference types).
    /// </param>
    /// <param name="fault">
    /// When this method returns <see langword="false"/>, contains the fault object; 
    /// otherwise, the default value of <typeparamref name="TFault"/> (which may be <see langword="null"/> for reference types).
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the result is a success and <paramref name="value"/> was retrieved; 
    /// <see langword="false"/> if the result is a failure and <paramref name="fault"/> was retrieved.
    /// </returns>
    public bool TryGetValue([MaybeNullWhen(false)] out TValue? value, [MaybeNullWhen(true)] out TFault? fault)
    {
        value = default;
        fault = default;

        if (Success)
        {
            value = _value;
            return true;
        }
        else
        {
            fault = _fault;
            return false;
        }
        
    }

    /// <summary>
    /// Attempts to retrieve the success value from the result.
    /// This is a convenience overload when you only need to handle the successful value and can ignore the fault.
    /// </summary>
    /// <param name="value">
    /// When this method returns <see langword="true"/>, contains the success value; 
    /// otherwise, the default value of <typeparamref name="TValue"/> (which may be <see langword="null"/> for reference types).
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the result is a success and <paramref name="value"/> was retrieved; 
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGetValue([MaybeNullWhen(false)] out TValue? value) => TryGetValue(out value, out _);

    /// <summary>
    /// Ensures the operation was a failure. If the result represents a success (<see cref="Success"/>),
    /// throws an exception created by the provided factory, passing the success value into it.
    /// This method is useful for asserting that a specific operation should have failed, throwing an exception
    /// if it unexpectedly succeeded.
    /// </summary>
    /// <param name="exceptionFactory">A factory function that creates an <see cref="Exception"/> based on the success value.</param>
    /// <exception cref="Exception">Thrown if the result is a <see cref="Success"/>.</exception>
    public void EnsureFailure(Func<TValue, Exception> exceptionFactory) { if (Success) throw exceptionFactory(_value!); }

    /// <summary>
    /// Ensures the operation was a failure. If the result represents a success (<see cref="Success"/>),
    /// throws an exception created by the provided factory. This method hides the base implementation.
    /// This overload is a simpler way to assert failure without needing to expose the success value directly in the factory.
    /// </summary>
    /// <param name="exceptionFactory">A factory function that creates an <see cref="Exception"/>.</param>
    /// <exception cref="Exception">Thrown if the result is a <see cref="Success"/>.</exception>
    public new void EnsureFailure(Func<Exception> exceptionFactory) => EnsureFailure(_ => exceptionFactory());

    /// <summary>
    /// Ensures the operation was a failure. If the result represents a success (<see cref="Success"/>),
    /// throws the provided exception. This method hides the base implementation.
    /// This overload allows you to throw a pre-defined exception directly if the result was unexpectedly successful.
    /// </summary>
    /// <param name="exception">The <see cref="Exception"/> to be thrown if the result is a <see cref="Success"/>.</param>
    /// <exception cref="Exception">Thrown if the result is a <see cref="Success"/>.</exception>
    public new void EnsureFailure(Exception exception) => EnsureFailure(_ => exception);

    /// <summary>
    /// Ensures the operation was a failure. If the result represents a success (<see cref="Success"/>),
    /// throws a <see cref="ResultWasSuccessException{TValue}"/> encapsulating the success value.
    /// This is a convenience overload for asserting failure using a default exception type specific to successful outcomes.
    /// This method hides the base implementation.
    /// </summary>
    /// <exception cref="ResultWasSuccessException{TValue}">Thrown if the result is a <see cref="Success"/>.</exception>
    public new void EnsureFailure() => EnsureFailure(value => new ResultWasSuccessException<TValue>(value));

    // <summary>
    /// Retrieves the fault object if the result represents a failure.
    /// If the result is a success (<see cref="Success"/>), it throws an exception created by the provided factory,
    /// passing the success value into it.
    /// This is particularly useful in scenarios where you are specifically expecting a failure, and a successful outcome
    /// signifies an unexpected state that should halt execution.
    /// </summary>
    /// <param name="exceptionFactory">A factory function that creates an <see cref="Exception"/> based on the success value.</param>
    /// <returns>The fault object if the result is a <see cref="Failure"/>.</returns>
    /// <exception cref="Exception">Thrown if the result is a <see cref="Success"/>.</exception>
    public TFault GetFaultOrThrow(Func<TValue, Exception> exceptionFactory) => Success ? throw exceptionFactory(_value!) : _fault!;

    /// <summary>
    /// Retrieves the fault object if the result represents a failure.
    /// If the result is a success (<see cref="Success"/>), it throws an exception created by the provided factory.
    /// This method hides the base implementation to provide a specific overload for the generic result type.
    /// </summary>
    /// <param name="exceptionFactory">A factory function that creates an <see cref="Exception"/> to be thrown if the result is a success.</param>
    /// <returns>The fault object if the result is a <see cref="Failure"/>.</returns>
    /// <exception cref="Exception">Thrown if the result is a <see cref="Success"/>.</exception>
    public new TFault GetFaultOrThrow(Func<Exception> exceptionFactory) => GetFaultOrThrow(_ => exceptionFactory());

    /// <summary>
    /// Retrieves the fault object if the result represents a failure.
    /// If the result is a success (<see cref="Success"/>), it throws the provided exception.
    /// This method hides the base implementation.
    /// </summary>
    /// <param name="exception">The <see cref="Exception"/> to be thrown if the result is a <see cref="Success"/>.</param>
    /// <returns>The fault object if the result is a <see cref="Failure"/>.</returns>
    /// <exception cref="Exception">Thrown if the result is a <see cref="Success"/>.</exception>
    public new TFault GetFaultOrThrow(Exception exception) => GetFaultOrThrow(_ => exception);

    /// <summary>
    /// Retrieves the fault object if the result represents a failure.
    /// If the result is a success (<see cref="Success"/>), it throws a <see cref="ResultWasSuccessException{TValue}"/>,
    /// encapsulating the success value.
    /// This method hides the base implementation and serves as a convenient default for throwing an exception on unexpected success.
    /// </summary>
    /// <returns>The fault object if the result is a <see cref="Failure"/>.</returns>
    /// <exception cref="ResultWasSuccessException{TValue}">Thrown if the result is a <see cref="Success"/>.</exception>
    public new TFault GetFaultOrThrow() => GetFaultOrThrow(_ => new ResultWasSuccessException<TValue>(_value!));

    /// <summary>
    /// Retrieves the success value if the result represents a success; otherwise, returns the default value for <typeparamref name="TValue"/>.
    /// This method provides a safe way to access the value without throwing an exception, making it suitable for scenarios
    /// where you want to handle the absence of a value explicitly.
    /// </summary>
    /// <returns>
    /// The success value if <see cref="Success"/>; 
    /// otherwise, <see langword="default"/> for <typeparamref name="TValue"/> (which may be <see langword="null"/> for reference types).
    /// </returns>
    public TValue? GetValueOrDefault() => _value;

    /// <summary>
    /// Retrieves the success value if the result represents a success.
    /// If the result is a failure (<see cref="Failure"/>), it throws an exception created by the provided factory,
    /// passing the fault object into it.
    /// This method is useful for quickly getting the value when you expect a success and want to signal an error
    /// if the operation unexpectedly failed.
    /// </summary>
    /// <param name="exceptionFactory">A factory function that creates an <see cref="Exception"/> based on the fault object.</param>
    /// <returns>The success value if the result is a <see cref="Success"/>.</returns>
    /// <exception cref="Exception">Thrown if the result is a <see cref="Failure"/>.</exception>
    public TValue GetValueOrThrow(Func<TFault, Exception> exceptionFactory) => Success ? _value! : throw exceptionFactory(_fault!);

    /// <summary>
    /// Retrieves the success value if the result represents a success.
    /// If the result is a failure (<see cref="Failure"/>), it throws an exception created by the provided factory.
    /// This overload provides a simpler way to assert success without needing to expose the fault object to the factory.
    /// </summary>
    /// <param name="exceptionFactory">A factory function that creates an <see cref="Exception"/> to be thrown if the result is a failure.</param>
    /// <returns>The success value if the result is a <see cref="Success"/>.</returns>
    /// <exception cref="Exception">Thrown if the result is a <see cref="Failure"/>.</exception>
    public TValue GetValueOrThrow(Func<Exception> exceptionFactory) => Success ? _value! : throw exceptionFactory();

    /// <summary>
    /// Retrieves the success value if the result represents a success.
    /// If the result is a failure (<see cref="Failure"/>), it throws the provided exception.
    /// This overload allows throwing a pre-defined exception directly when a success is expected but failure occurs.
    /// </summary>
    /// <param name="exception">The <see cref="Exception"/> to be thrown if the result is a <see cref="Failure"/>.</param>
    /// <returns>The success value if the result is a <see cref="Success"/>.</returns>
    /// <exception cref="Exception">Thrown if the result is a <see cref="Failure"/>.</exception>
    public TValue GetValueOrThrow(Exception exception) => GetValueOrThrow(_ => exception);

    /// <summary>
    /// Retrieves the success value if the result represents a success.
    /// If the result is a failure (<see cref="Failure"/>), it throws a <see cref="ResultWasFailureException{TFault}"/>,
    /// encapsulating the fault object.
    /// This is a convenience overload for asserting that the operation should have succeeded, throwing a default exception on failure.
    /// </summary>
    /// <returns>The success value if the result is a <see cref="Success"/>.</returns>
    /// <exception cref="ResultWasFailureException{TFault}">Thrown if the result is a <see cref="Failure"/>.</exception>
    public TValue GetValueOrThrow() => GetValueOrThrow(_ => new ResultWasFailureException<TFault>(_fault!));
}