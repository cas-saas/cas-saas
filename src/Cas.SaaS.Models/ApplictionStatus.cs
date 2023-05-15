namespace Cas.SaaS.Models;

/// <summary>
/// Статус заявки
/// </summary>
public enum ApplicationStatus
{
    /// <summary>
    /// Новая
    /// </summary>
    New,

    /// <summary>
    /// Рассматривается
    /// </summary>
    InProgress,

    /// <summary>
    /// Отклонена
    /// </summary>
    Rejected,

    /// <summary>
    /// Одобрена
    /// </summary>
    Approved,
}
