using Cas.SaaS.Models;

namespace Cas.SaaS.Contracts.Brigade;

/// <summary>
/// Модель вывода наряда
/// </summary>
public class BrigadeDto
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
}
