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
    /// Принадлежность сотрудника к клиенту
    /// </summary>
    public virtual Client Client { get; set; } = null!;
    /// <summary>
    /// Наряды сотрудника
    /// </summary>
    public virtual IEnumerable<Brigade> Brigades { get; set; } = null!;
}
