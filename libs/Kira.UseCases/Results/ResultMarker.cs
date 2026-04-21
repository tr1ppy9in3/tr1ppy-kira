using Kira.UseCases.Results.Markers;

namespace Kira.UseCases.Results;

public static class ResultMarker
{
    /// <summary>
    /// Creates a default, parameterless success marker. This is typically used when an operation
    /// succeeds without producing a specific return value (e.g., for void operations).
    /// </summary>
    /// <returns>A default instance of <see cref="SuccessMarker"/>.</returns>
    public static SuccessMarker Succeed()
    {
        return default;
    }

    /// <summary>
    /// Creates a success marker encapsulating a specific value.
    /// This is used to indicate a successful operation that yields a result of type <typeparamref name="TValue"/>.
    /// </summary>
    /// <typeparam name="TValue">The type of the successful value.</typeparam>
    /// <param name="value">The value to be encapsulated in the success marker.</param>
    /// <returns>A new <see cref="SuccessMarker{TValue}"/> instance containing the provided value.</returns>
    public static SuccessMarker<TValue> Succeed<TValue>(TValue value)
    {
        return new(value);
    }

    /// <summary>
    /// Creates a failure marker encapsulating a specific fault object.
    /// This is used to indicate a failed operation with a detailed fault reason of type <typeparamref name="TFault"/>.
    /// </summary>
    /// <typeparam name="TFault">The type of the fault object.</typeparam>
    /// <param name="fault">The fault object indicating the reason for failure.</param>
    /// <returns>A new <see cref="FailureMarker{TFault}"/> instance containing the provided fault.</returns>
    public static FailureMarker<TFault> Fail<TFault>(TFault fault)
    {
        return new(fault);
    }
}
