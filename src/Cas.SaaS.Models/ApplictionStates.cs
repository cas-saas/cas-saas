namespace Cas.SaaS.Models;

/// <summary>
/// Статус заявки
/// </summary>
public enum ApplicationStates
{
    /// <summary>
    /// Новая
    /// </summary>
    New = 0,

    /// <summary>
    /// Рассматривается
    /// </summary>
    InProgress = 1,

    /// <summary>
    /// Отклонена
    /// </summary>
    Rejected = 2,

    /// <summary>
    /// Одобрена
    /// </summary>
    Approved = 3,
}
