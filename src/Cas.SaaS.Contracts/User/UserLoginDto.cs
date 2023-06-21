using System.ComponentModel.DataAnnotations;

namespace Cas.SaaS.Contracts.User;

/// <summary>
/// Модель входа в систему
/// </summary>
public class UserLoginDto
{
    [Required(ErrorMessage = "Введите логин!")]
    public string? Login { get; set; }

    [Required(ErrorMessage = "Введите пароль!")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
}

/// <summary>
/// Модель результата входа в систему
/// </summary>
public class UserLoginResultDTO
{
    /// <summary>
    /// Результат
    /// </summary>
    public bool Succeeded { get; set; }

    /// <summary>
    /// Сообщение сервера
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Токен
    /// </summary>
    public TokenDTO Token { get; set; } = null!;
}

/// <summary>
/// Модель получения токена
/// </summary>
public class TokenDTO
{
    /// <summary>
    /// Токен
    /// </summary>
    public string? AccessToken { get; set; }

    /// <summary>
    /// Время
    /// </summary>
    public DateTime Expiration { get; set; }
}