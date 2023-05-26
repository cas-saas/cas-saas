namespace Cas.SaaS.Models;

/// <summary>
/// Статус наряда
/// </summary>
public enum BrigadeStates
{
    /// <summary>
    /// Новая
    /// </summary>
    New = 0,

    /// <summary>
    /// В работе
    /// </summary>
    InProgress = 1,

    /// <summary>
    /// Завершена
    /// </summary>
    Completed = 2,

    /// <summary>
    /// Отменена клиентом
    /// </summary>
    CanceledByClient = 3,

    /// <summary>
    /// Отклонена сотрудником
    /// </summary>
    RejectedByEmployee = 4,
}
