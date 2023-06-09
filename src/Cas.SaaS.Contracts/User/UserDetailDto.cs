using Cas.SaaS.Contracts.Delivery;
using Cas.SaaS.Contracts.Employee;
using Cas.SaaS.Models;

namespace Cas.SaaS.Contracts.User;

/// <summary>
/// Модель вывода деталей клиента
/// </summary>
public class UserDetailDto
{
    /// <summary>
    /// Идентификатор пользователя
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
    /// Статус пользователя
    /// </summary>
    public ClientStatus Status { get; set; } = ClientStatus.None;

    /// <summary>
    /// Сотрудники клиент
    /// </summary>
    public List<EmployeeDto> Employees { get; set; } = null!;

    /// <summary>
    /// Заказы клиента
    /// </summary>
    public List<DeliveryDto> Deliveries { get; set; } = null!;
}
