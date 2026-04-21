namespace Kira.UseCases.Results.Markers;

/// <summary>
/// Represents a marker type for a failed operation that includes a specific fault object.
/// This record struct is designed for use with implicit conversion operators
/// to simplify the creation of <see cref="ResultMarker"/> instances
/// when an operation fails with an error.
/// </summary>
/// <typeparam name="TFault">The type of the fault or error that occurred.</typeparam>
/// <param name="Fault">The encapsulated fault object.</param>
public readonly record struct FailureMarker<TFault>(TFault Fault);