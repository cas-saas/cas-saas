namespace Cas.SaaS.Contracts.Brigade;

public class BrigadeAddDto
{
    /// <summary>
    /// Идентификатор услуги
    /// </summary>
    public Guid ServiceId { get; set; }

    /// <summary>
    /// Идентификаторы сотрудников
    /// </summary>
    public List<Guid> EmployeesId { get; set; } = null!;

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
