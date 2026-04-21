namespace Kira.UseCases.Faults;

public interface IValidatableFault<out TSelf> 
{
    static abstract TSelf ValidationError(string[] errors, string message = "Ошибка валидации");
    static abstract TSelf InternalError(string message = "Внутренняя ошибка сервера");
}