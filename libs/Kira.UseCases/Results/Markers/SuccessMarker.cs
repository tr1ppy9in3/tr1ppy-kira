namespace Kira.UseCases.Results.Markers;

/// <summary>
/// Represents a marker type for a successful operation that does not produce a specific return value (i.e., a 'void' success).
/// This <see langword="ref struct"/> is typically used with implicit conversion operators
/// to simplify the creation of <see cref="ResultMarker"/> (where TValue might be a Unit type)
/// or similar result types when only the success state itself is relevant.
/// </summary>
public readonly ref struct SuccessMarker;