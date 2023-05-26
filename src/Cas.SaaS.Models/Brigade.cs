namespace Cas.SaaS.Models;

/// <summary>
/// Модель наряда
/// </summary>
public class Brigade
{
    /// <summary>
    /// Идентификатор наряда в системе
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Статус наряда
    /// </summary>
    public BrigadeStates Status { get; set; }

    /// <summary>
    /// Идентификатор услуги
    /// </summary>
    public Guid ServiceId { get; set; }

    /// <summary>
    /// Оказываемая услуга
    /// </summary>
    public virtual Service Service { get; set; } = null!;

    /// <summary>
    /// Выездные сотрудники
    /// </summary>
    public virtual IEnumerable<Employee> Employees { get; set; } = new List<Employee>();

    /// <summary>
    /// Заказчик ФИО
    /// </summary>
    public string Customer { get; set; } = string.Empty;

    /// <summary>
    /// Номер заказчика
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Адрес заказа
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Комментраии к наряду
    /// </summary>
    public string Description { get; set; } = string.Empty;
}
