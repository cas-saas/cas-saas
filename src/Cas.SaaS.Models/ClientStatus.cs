using System.ComponentModel.DataAnnotations;

namespace Cas.SaaS.Models;

/// <summary>
/// Статус клиента
/// </summary>
public enum ClientStatus
{
    /// <summary>
    /// Заблокирован
    /// </summary>
    [Display(Name = "Заблокирован")]
    Blocked = 0,

    /// <summary>
    /// Оплачен
    /// </summary>
    [Display(Name = "Оплачен")] 
    Paid = 1,

    /// <summary>
    /// Не оплачен
    /// </summary>
    [Display(Name = "Не оплачен")]
    NotPaid = 2,

    /// <summary>
    /// Отсутствует
    /// </summary>
    [Display(Name = "Отсутствует")]
    None = 3,
}
