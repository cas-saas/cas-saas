namespace Cas.SaaS.Models;

/// <summary>
/// Статус наряда
/// </summary>
public enum BrigadeStates
{
    /// <summary>
    /// Новая
    /// </summary>
    New,

    /// <summary>
    /// В работе
    /// </summary>
    InProgress,

    /// <summary>
    /// Завершена
    /// </summary>
    Completed,

    /// <summary>
    /// Отменена клиентом
    /// </summary>
    CanceledByClient,

    /// <summary>
    /// Отклонена сотрудником
    /// </summary>
    RejectedByEmployee,
}
