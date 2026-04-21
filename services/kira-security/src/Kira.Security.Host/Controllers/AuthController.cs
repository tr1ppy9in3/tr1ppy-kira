using Kira.Security.Host.Mapping;
using Kira.Security.Identity.UseCases.Features.Registration.Commands.VerifyRegistrationState;
using MediatR;
using Microsoft.AspNetCore.Mvc;


using System.ComponentModel.DataAnnotations;
using Kira.Security.Authentication.UseCases.Features.Login.Commands;
using Kira.Security.Authentication.UseCases.Features.Login.Commands.LoginByCode;
using Kira.Security.Authentication.UseCases.Features.Login.Commands.LoginByEmail;
using Kira.Security.Authentication.UseCases.Features.Login.Commands.LoginByLogin;
using Kira.Security.Authentication.UseCases.Features.Login.Commands.SendOneTimeLoginCode;
using Kira.Security.Authentication.UseCases.Features.Session.Commands.LogoutCommand;
using Kira.Security.Authentication.UseCases.Features.Session.Commands.RefreshCommand;
using Kira.Security.Identity.UseCases.Features.RecoveryPassword.Commands.ResolveRecoveryPasswordCode;
using Kira.Security.Identity.UseCases.Features.RecoveryPassword.Commands.SendRecoveryPasswordCode;
using Kira.Security.Identity.UseCases.Features.Registration.Commands.CreateRegistrationState;

namespace Kira.Security.Host.Controllers;

/// <summary>
/// Контроллер аутентификации и регистрации пользователей.
/// Отвечает за вход в систему, регистрацию и выдачу токенов доступа.
/// </summary>
[ApiController]
[Route("api/v1/auth")]
[Produces("application/json")]
public sealed class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Инициализирует новый экземпляр контроллера <see cref="AuthController"/>.
    /// </summary>
    /// <param name="mediator">Медиатор для маршрутизации команд и запросов.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если mediator равен null.</exception>
    public AuthController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    #region Registration (Регистрация)

    /// <summary>
    /// Шаг 1. Инициирует процесс регистрации нового пользователя.
    /// Создает временную сессию и отправляет код подтверждения на указанный Email.
    /// </summary>
    /// <param name="command">Данные для регистрации (Логин, Email, Пароль).</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Статус успешного начала регистрации (204 No Content).</returns>
    [HttpPost("register/start")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)] // EmailAlreadyTaken
    public async Task<IActionResult> StartRegistration(
        [FromBody, Required] CreateRegistrationStateCommand command, 
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.ToActionResult(() => NoContent());
    }

    /// <summary>
    /// Шаг 2. Завершает процесс регистрации.
    /// Проверяет код из письма и создает учетную запись пользователя.
    /// </summary>
    /// <param name="command">Данные для подтверждения (Email, Код).</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Статус успешной регистрации (200 OK).</returns>
    [HttpPost("register/verify")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)] // InvalidCode
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status410Gone)]       // TimeExpired
    public async Task<IActionResult> VerifyRegistration(
        [FromBody, Required] VerifyRegistrationStateCommand command, 
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.ToActionResult(() => Ok());
    }

    #endregion

    #region Login (Аутентификация)

    /// <summary>
    /// Вход в систему по электронной почте и паролю.
    /// </summary>
    /// <param name="command">Учетные данные (Email, Пароль).</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Пара токенов (Access и Refresh).</returns>
    [HttpPost("login/email")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)] // BadCredentials
    public async Task<IActionResult> LoginByEmail(
        [FromBody, Required] LoginByEmailCommand command, 
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.ToOkActionResult(); 
    }

    /// <summary>
    /// Вход в систему по логину (никнейму) и паролю.
    /// </summary>
    /// <param name="command">Учетные данные (Логин, Пароль).</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Пара токенов (Access и Refresh).</returns>
    [HttpPost("login/username")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> LoginByUsername(
        [FromBody, Required] LoginByLoginCommand command, 
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.ToOkActionResult();
    }

    #endregion

    #region One-Time Password (OTP)

    /// <summary>
    /// Запрашивает одноразовый код (OTP) для входа в систему.
    /// Код отправляется на указанный Email, если пользователь существует.
    /// </summary>
    /// <param name="command">Email пользователя.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Всегда возвращает 202 Accepted в целях безопасности (защита от перебора email).</returns>
    [HttpPost("login/otp/request")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> RequestOneTimeCode(
        [FromBody, Required] SendOneTimeLoginCodeCommand command, 
        CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return Accepted();
    }

    /// <summary>
    /// Вход в систему по одноразовому коду (OTP), полученному на Email.
    /// </summary>
    /// <param name="command">Данные для входа (Email, Код).</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Пара токенов (Access и Refresh).</returns>
    [HttpPost("login/otp/verify")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)] // Invalid/Expired Code
    public async Task<IActionResult> LoginByOneTimeCode(
        [FromBody, Required] LoginByCodeCommand command, 
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.ToOkActionResult();
    }

    #endregion
    
    /// <summary>
    /// Обновляет истекший Access Token, используя валидный Refresh Token.
    /// </summary>
    [HttpPost("session/refresh")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenCommand command)
    {
        var result = await _mediator.Send(command);
        return result.ToOkActionResult();
    }

    /// <summary>
    /// Завершает сессию пользователя, удаляя токен обновления.
    /// </summary>
    [HttpPost("session/logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Logout([FromBody] LogoutCommand command)
    {
        var result = await _mediator.Send(command);
        return result.ToActionResult(() => NoContent());
    }
    
        #region Password Recovery (Восстановление пароля)

    /// <summary>
    /// Запрашивает код для восстановления пароля.
    /// Код будет отправлен на Email, если пользователь зарегистрирован в системе.
    /// </summary>
    /// <param name="command">Данные запроса (Email).</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Всегда возвращает 202 Accepted для предотвращения перебора Email.</returns>
    [HttpPost("recovery/request")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SendRecoveryCode(
        [FromBody, Required] SendRecoveryPasswordCodeCommand command,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return Accepted();
    }

    /// <summary>
    /// Устанавливает новый пароль, используя код подтверждения из письма.
    /// </summary>
    /// <param name="command">Данные для смены пароля (Email, Код, Новый пароль).</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Статус успешного обновления пароля (204 No Content).</returns>
    [HttpPost("recovery/resolve")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)] // Неверный код или формат пароля
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status410Gone)]       // Код протух
    public async Task<IActionResult> ResolveRecovery(
        [FromBody, Required] ResolveRecoveryPasswordCodeCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.ToActionResult(() => NoContent());
    }

    #endregion
}