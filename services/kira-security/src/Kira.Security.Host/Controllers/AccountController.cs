using Kira.Security.Host.Mapping;
using Kira.Security.Management.UseCases.Features.Account.Commands.ChangePassword;
using Kira.Security.Management.UseCases.Features.Account.Commands.UpdateProfile;
using Kira.Security.Management.UseCases.Features.Account.Dtos;
using Kira.Security.Management.UseCases.Features.Account.Queries.GetProfileQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kira.Security.Host.Controllers;

/// <summary>
/// Контроллер управления аккаунтом текущего пользователя.
/// </summary>
[Authorize]
[ApiController]
[Route("api/v1/account")]
[Produces("application/json")]
public sealed class AccountController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    /// <summary>
    /// Получить данные профиля текущего пользователя.
    /// </summary>
    /// <returns>Данные профиля (имя, фамилия, аватар и т.д.).</returns>
    [HttpGet("profile")]
    [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfile(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetProfileQuery(), ct);
        return result.ToOkActionResult();
    }

    /// <summary>
    /// Обновить данные профиля.
    /// </summary>
    /// <param name="model">Новые данные профиля.</param>
    /// <param name="ct">Токен отмены.</param>
    /// <returns>Статус успешного обновления (204 No Content).</returns>
    [HttpPut("profile")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileCommandDto model, CancellationToken ct)
    {
        var command = new UpdateProfileCommand(model);
        var result = await _mediator.Send(command, ct);
        
        return result.ToActionResult(NoContent);
    }

    /// <summary>
    /// Сменить пароль пользователя.
    /// После смены пароля рекомендуется обновить токен доступа.
    /// </summary>
    /// <param name="model">Данные для смены пароля (старый и новый пароли).</param>
    /// <param name="ct">Токен отмены.</param>
    /// <returns>Статус успешной смены пароля (204 No Content).</returns>
    [HttpPost("password/change")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommandDto model, CancellationToken ct)
    {
        var result = await _mediator.Send(new ChangePasswordCommand(model), ct);
        return result.ToActionResult(NoContent);
    }
}