using Kira.UseCases.Results.Abstractions;
using Kira.UseCases.Results.Markers;

namespace Kira.UseCases.Results.Implementations;
/// <summary>
/// Based realization of <see cref="BaseResult{TFault}"/>
/// </summary>
/// <typeparam name="TFault"> Type of fault. </typeparam>
public sealed class Result<TFault> : BaseResult<TFault>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ResultMarker"/> class representing a successful operation.
    /// </summary>
    public Result() : base() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResultMarker"/> class representing a failed operation.
    /// </summary>
    /// <param name="fault">The fault object indicating the reason for failure.</param>
    public Result(TFault fault) : base(fault) { }

    public static Result<TFault> Succeed()
        => new();

    public static Result<TFault> Fail(TFault fault)
        => new(fault);

    public static implicit operator Result<TFault>(TFault fault)
        => Fail(fault);

    public static implicit operator Result<TFault>(SuccessMarker successMarker)
        => Succeed();

    public static implicit operator Result<TFault>(FailureMarker<TFault> failureMarker)
        => Fail(failureMarker.Fault);
}
