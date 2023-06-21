using System.ComponentModel.DataAnnotations;

namespace Cas.SaaS.Contracts.Client;

/// <summary>
/// Модель добавления клиента
/// </summary>
public class ClientAddDto
{
    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string Login { get; set; } = string.Empty;

    /// <summary>
    /// Пароль пользователя
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Номер телефона пользователя
    /// </summary>
    [Required(ErrorMessage = "Введите номер телефона!")] 
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Почтовый адрес пользователя
    /// </summary>
    [Required(ErrorMessage = "Введите почтовый адрес!")] 
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Имя
    /// </summary>
    [Required(ErrorMessage = "Введите имя!")] 
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Фамилия
    /// </summary>
    [Required(ErrorMessage = "Введите фамилию!")] 
    public string Surname { get; set; } = string.Empty;

    /// <summary>
    /// Отчество
    /// </summary>
    public string? Patronymic { get; set; }
}