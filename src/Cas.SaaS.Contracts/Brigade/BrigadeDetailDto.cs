using Cas.SaaS.Contracts.Employee;
using Cas.SaaS.Models;

namespace Cas.SaaS.Contracts.Brigade;

/// <summary>
/// Модель вывода деталей наряда
/// </summary>
public class BrigadeDetailDto
{
    /// <summary>
    /// Идентификатор наряда в системе
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Номер наряда
    /// </summary>
    public string NumberBrigade { get; set; } = string.Empty;

    /// <summary>
    /// Статус наряда
    /// </summary>
    public BrigadeStates Status { get; set; }

    /// <summary>
    /// Идентификатор услуги
    /// </summary>
    public Guid ServiceId { get; set; }

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

    /// <summary>
    /// Дата начала работы
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Дата окончания работы
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Ответственные за наряд
    /// </summary>
    public List<EmployeeDetailDto> EmployeesId { get; set; } = null!;
}
