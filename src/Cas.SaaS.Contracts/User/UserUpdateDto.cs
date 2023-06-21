using Cas.SaaS.Contracts.Delivery;
using Cas.SaaS.Contracts.Employee;
using Cas.SaaS.Models;
using System.ComponentModel.DataAnnotations;

namespace Cas.SaaS.Contracts.User;

/// <summary>
/// Модель обновления клиента
/// </summary>
public class UserUpdateDto
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid Id { get; set; }

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

    /// <summary>
    /// Статус пользователя
    /// </summary>
    public ClientStatus Status { get; set; } = ClientStatus.None;
}
