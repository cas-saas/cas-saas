namespace Cas.SaaS.Models;

/// <summary>
/// Модель клиента
/// </summary>
public class Client : User
{
    /// <summary>
    /// Имя клиента
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Фамилия клиента
    /// </summary>
    public string Surname { get; set; } = string.Empty;
    /// <summary>
    /// Отчество клиента
    /// </summary>
    public string? Patronymic { get; set; }
    public ClientStatus Status { get; set; }
    /// <summary>
    /// Список сотрудников клиента
    /// </summary>
    public virtual IEnumerable<Employee> Employees { get; set; } = new List<Employee>();
    /// <summary>
    /// Список заказов клиента
    /// </summary>
    public virtual IEnumerable<Delivery> Deliveries { get; set; } = new List<Delivery>();
}