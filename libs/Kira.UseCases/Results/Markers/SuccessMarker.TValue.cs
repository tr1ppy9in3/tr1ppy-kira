namespace Kira.UseCases.Results.Markers;

/// <summary>
/// Represents a marker type for a successful operation that produces a specific value.
/// This record struct is designed for use with implicit conversion operators
/// to simplify the creation of <see cref="ResultMarker"/> instances
/// when an operation succeeds with a value.
/// </summary>
/// <typeparam name="TValue">The type of the successful value.</typeparam>
/// <param name="Value">The encapsulated success value.</param>
public readonly record struct SuccessMarker<TValue>(TValue Value);