using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Cas.SaaS.Models;

/// <summary>
/// Роли пользователей
/// </summary>
public enum UserRoles
{
    /// <summary>
    /// Клиент
    /// </summary>
    [Display(Name = "Клиент")]
    Client = 0,

    /// <summary>
    /// Сотрудник
    /// </summary>
    [Display(Name = "Сотрудник")]
    Employee = 1,

    /// <summary>
    /// Администратор
    /// </summary>
    [Display(Name = "Администратор")]
    Admin = 2,
}
