using Cas.SaaS.Models;
using System.ComponentModel.DataAnnotations;

namespace Cas.SaaS.Contracts.Application;

/// <summary>
/// Модель изменения просмотра заявки
/// </summary>
public class ApplicationIsCheckDto
{
    /// <summary>
    /// Была ли просмотренна заявка
    /// </summary>
    public bool IsCheck { get; set; } = true;
}
