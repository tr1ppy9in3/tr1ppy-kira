namespace Kira.UseCases.Faults;

public abstract record CommandFault(string Message, string[]? Errors = null)
{
    public const string GeneralNamespace = "General";
    
    public bool IsGeneral { get; init; } = false;
    protected string? _generalCode;

    public string Namespace => IsGeneral ? GeneralNamespace : GetModuleNamespace();
    public string Code => IsGeneral ? _generalCode! : GetModuleCode();
    public string FullCode => $"{Namespace}.{Code}";

    protected abstract string GetModuleNamespace();
    protected abstract string GetModuleCode();
    
    public static CommandFault ValidationError(string[] errors, string message = "Ошибка валидации") =>
        new SystemFault("ValidationError", message, errors);

    public static CommandFault InternalError(string message = "Внутренняя ошибка сервера") =>
        new SystemFault("InternalError", message);
}

public abstract record CommandFault<TEnum, TSelf> : CommandFault, IValidatableFault<TSelf>
    where TEnum : struct, Enum 
    where TSelf : CommandFault<TEnum, TSelf>, new()
{
    public TEnum? Type { get; protected set; }

    protected CommandFault() : base(string.Empty) { }

    protected override string GetModuleCode() => Type?.ToString() ?? "Unknown";

    public static TSelf Create(TEnum type, string message) => 
        new() { Type = type, Message = message, IsGeneral = false };
    
    public static TSelf Create(TEnum type, string message, string[]? errors) => 
        new() { Type = type, Message = message, IsGeneral = false, Errors = errors };

    public new static TSelf ValidationError(string[] errors, string message = "Ошибка валидации") =>
        new() { IsGeneral = true, _generalCode = "ValidationError", Message = message, Errors = errors };

    public new static TSelf InternalError(string message = "Внутренняя ошибка сервера") =>
        new() { IsGeneral = true, _generalCode = "InternalError", Message = message };
}

internal record SystemFault : CommandFault
{
    public SystemFault(string generalCode, string message, string[]? errors = null) 
        : base(message, errors)
    {
        IsGeneral = true;
        _generalCode = generalCode;
    }

    protected override string GetModuleNamespace() => GeneralNamespace;
    protected override string GetModuleCode() => _generalCode!;
}