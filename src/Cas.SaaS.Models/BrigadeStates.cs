using System.ComponentModel.DataAnnotations;

namespace Cas.SaaS.Models;

/// <summary>
/// Статус наряда
/// </summary>
public enum BrigadeStates
{
    /// <summary>
    /// Новая
    /// </summary>
    [Display(Name = "Новая")]
    New = 0,

    /// <summary>
    /// В работе
    /// </summary>
    [Display(Name = "В работе")]
    InProgress = 1,

    /// <summary>
    /// Завершена
    /// </summary>
    [Display(Name = "Завершена")]
    Completed = 2,

    /// <summary>
    /// Отменена клиентом
    /// </summary>
    [Display(Name = "Отменена клиентом")]
    CanceledByClient = 3,

    /// <summary>
    /// Отклонена сотрудником
    /// </summary>
    [Display(Name = "Отклонена сотрудником")]
    RejectedByEmployee = 4,
}
