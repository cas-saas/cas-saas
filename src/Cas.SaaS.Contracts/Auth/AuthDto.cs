using System.ComponentModel.DataAnnotations;

namespace Cas.SaaS.Contracts.Auth;

/// <summary>
/// Модель авторизации
/// </summary>
public class AuthDto
{
    /// <summary>
    /// Логин пользователя
    /// </summary>
    [Required(ErrorMessage = "Введите логин!")]
    public string? Login { get; set; }

    /// <summary>
    /// Пароль пользователя
    /// </summary>
    [Required(ErrorMessage = "Введите пароль!")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
}
