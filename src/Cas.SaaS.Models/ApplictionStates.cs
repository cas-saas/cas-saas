using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Cas.SaaS.Models;

/// <summary>
/// Статус заявки
/// </summary>
public enum ApplicationStates
{
    /// <summary>
    /// Новая
    /// </summary>
    [Display(Name = "Новая")]
    New = 0,

    /// <summary>
    /// Рассматривается
    /// </summary>
    [Display(Name = "Рассматривается")]
    InProgress = 1,

    /// <summary>
    /// Отклонена
    /// </summary>
    [Display(Name = "Отклонена")]
    Rejected = 2,

    /// <summary>
    /// Одобрена
    /// </summary>
    [Display(Name = "Одобрена")]
    Approved = 3
}