namespace Cas.SaaS.Models;

/// <summary>
/// Статус клиента
/// </summary>
public enum ClientStatus
{
    /// <summary>
    /// Заблокирован
    /// </summary>
    Blocked = 0,

    /// <summary>
    /// Оплачен
    /// </summary>
    Paid = 1,

    /// <summary>
    /// Не оплачен
    /// </summary>
    NotPaid = 2
}
