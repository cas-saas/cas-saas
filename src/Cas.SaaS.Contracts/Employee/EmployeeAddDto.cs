using Cas.SaaS.Models;

namespace Cas.SaaS.Contracts.Employee;

/// <summary>
/// Модель добавления сотрудника
/// </summary>
public class EmployeeAddDto
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
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Почтовый адрес пользователя
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Имя
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Фамилия
    /// </summary>
    public string Surname { get; set; } = string.Empty;

    /// <summary>
    /// Отчество
    /// </summary>
    public string? Patronymic { get; set; }
}