using Cas.SaaS.API.Helpers;
using Cas.SaaS.API.Options;
using Cas.SaaS.Contracts.Auth;
using Cas.SaaS.Contracts.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

namespace Cas.SaaS.API.Controllers;

/// <summary>
/// Контроллер работы с авторизацией
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly DatabaseContext _context;
    private readonly IOptions<JwtOptions> _jwtOptions;
    private readonly JwtHelper _jwtHelper;
    private readonly ILogger<AuthController> _logger;

    /// <summary>
    /// Конструктор класса <see cref="AuthController"/>
    /// </summary>
    /// <param name="context">Контекст базы данных</param>
    /// <param name="jwtOptions">Настройки Jwt-токена</param>
    /// <param name="jwtHelper">Класс-помощник работы с Jwt</param>
    public AuthController(DatabaseContext context, IOptions<JwtOptions> jwtOptions, 
        JwtHelper jwtHelper, ILogger<AuthController> logger)
    {
        _context = context;
        _jwtOptions = jwtOptions;
        _jwtHelper = jwtHelper;
        _logger = logger;
    }

    /// <summary>
    /// Авторизовать пользователя
    /// </summary>
    /// <param name="authDto">Данные по пользователю</param>
    /// <returns>Токены</returns>
    [HttpPost]
    public async Task<IActionResult> Auth([FromBody] AuthDto authDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(item => item.Login == authDto.Login);
        if (user is null) return NotFound("Пользователь не найден!");

        // if (user.Blocked) return Conflict("Ошибка! Попытка авторизоваться заблокированному пользователю.");
        if (user.Password != authDto.Password) return Conflict("Ошибка! Неверный пароль!");

        user.RefreshToken = JwtHelper.CreateRefreshToken();
        user.RefreshTokenExpires = DateTime.UtcNow.AddSeconds(_jwtOptions.Value.RefreshTokenLifetime);

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        var tokens = new TokensDto
        {
            AccessToken = _jwtHelper.CreateAccessToken(user, _jwtOptions.Value.AccessTokenLifetime),
            RefreshToken = user.RefreshToken,
        };

        return Ok(new UserLoginResultDTO
        {
            Succeeded = true,
            Token = new TokenDTO
            {
                AccessToken = tokens.AccessToken,
                Expiration = (DateTime)user.RefreshTokenExpires
            }
        });
    }

    /// <summary>
    /// Обновляет токены с использованием валидного токена обновления. 
    /// </summary>
    /// <param name="refreshTokensDto">Данные токена обновления</param>
    /// <response code="200">Токены успешно обновлены</response>
    /// <response code="400">Передан некорректный токен обновления</response>
    /// <response code="401">Токен обновления устарел</response>
    /// <response code="500">Ошибка сервера</response>
    [HttpPost("refresh")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokensDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(RefreshTokensDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RefreshTokens([FromBody] RefreshTokensDto refreshTokensDto)
    {
        if (string.IsNullOrEmpty(refreshTokensDto.RefreshToken))
            return BadRequest($"Поле {nameof(refreshTokensDto.RefreshToken)} не может быть пустым");

        var user = _context.Users.FirstOrDefault(c => c.RefreshToken == refreshTokensDto.RefreshToken);
        if (user is null)
            return NotFound($"Пользователь с токеном обновления {refreshTokensDto.RefreshToken} не найден");

        if (user.RefreshTokenExpires < DateTime.UtcNow)
            return Unauthorized($"Токен обновления устарел");

        user.RefreshToken = JwtHelper.CreateRefreshToken();
        user.RefreshTokenExpires = DateTime.UtcNow.AddMinutes(_jwtOptions.Value.RefreshTokenLifetime);
        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        var tokens = new TokensDto
        {
            AccessToken = _jwtHelper.CreateAccessToken(user, _jwtOptions.Value.AccessTokenLifetime),
            RefreshToken = user.RefreshToken,
        };
        return Ok(tokens);
    }
}