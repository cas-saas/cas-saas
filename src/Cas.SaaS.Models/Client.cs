namespace Cas.SaaS.Models;

/// <summary>
/// Модель клиента
/// </summary>
public class Client : User
{
    /// <summary>
    /// Статус клиента
    /// </summary>
    public ClientStatus Status { get; set; } = ClientStatus.None;
    /// <summary>
    /// Список сотрудников клиента
    /// </summary>
    public virtual IEnumerable<Employee> Employees { get; set; } = null!;
    /// <summary>
    /// Список заказов клиента
    /// </summary>
    public virtual IEnumerable<Delivery> Deliveries { get; set; } = null!;
    /// <summary>
    /// Список услуг клиента
    /// </summary>
    public virtual IEnumerable<Service> Services { get; set; } = null!;
}