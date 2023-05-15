namespace Cas.SaaS.Models;

/// <summary>
/// Статус клиента
/// </summary>
public enum ClientStatus
{
    /// <summary>
    /// Заблокирован
    /// </summary>
    Blocked,

    /// <summary>
    /// Оплачен
    /// </summary>
    Paid,

    /// <summary>
    /// Не оплачен
    /// </summary>
    NotPaid
}
