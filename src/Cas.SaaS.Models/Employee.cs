namespace Cas.SaaS.Models;

/// <summary>
/// Модель сотрудника
/// </summary>
public class Employee : User
{
    /// <summary>
    /// Имя сотрудника
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Фамилия сотрудника
    /// </summary>
    public string Surname { get; set; } = string.Empty;
    /// <summary>
    /// Отчество сотрудника
    /// </summary>
    public string? Patronymic { get; set; }
    /// <summary>
    /// Активен ли сотрудник
    /// </summary>
    public bool IsActive { get; set; } = false;
    /// <summary>
    /// Принадлежность сотрудника к клиенту
    /// </summary>
    public virtual Client Client { get; set; } = null!;
}
