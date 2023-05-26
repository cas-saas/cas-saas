using Cas.SaaS.Models;

namespace Cas.SaaS.Contracts.Employee;

/// <summary>
/// Модель вывода клиента
/// </summary>
public class EmployeeDto
{
    /// <summary>
    /// Идентификатор сотрудника
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string Login { get; set; } = string.Empty;

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

    /// <summary>
    /// Роль пользователя
    /// </summary>
    public UserRoles Role { get; set; }

    /// <summary>
    /// Активен ли сотрудник
    /// </summary>
    public bool IsActive { get; set; } = false;
}
