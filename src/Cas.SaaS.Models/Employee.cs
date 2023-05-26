namespace Cas.SaaS.Models;

/// <summary>
/// Модель сотрудника
/// </summary>
public class Employee : User
{
    /// <summary>
    /// Активен ли сотрудник
    /// </summary>
    public bool IsActive { get; set; } = false;

    /// <summary>
    /// Идентификатор клиента
    /// </summary>
    public Guid ClientId { get; set; }

    /// <summary>
    /// Клиент
    /// </summary>
    public virtual Client Client { get; set; } = null!;

    /// <summary>
    /// Наряды сотрудника
    /// </summary>
    public virtual IEnumerable<Brigade> Brigades { get; set; } = null!;
}
