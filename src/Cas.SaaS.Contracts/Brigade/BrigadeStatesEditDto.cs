using Cas.SaaS.Models;
using System.ComponentModel.DataAnnotations;

namespace Cas.SaaS.Contracts.Brigade;

/// <summary>
/// Модель изменения состояния заявки
/// </summary>
public class BrigadeStatesEditDto
{
    /// <summary>
    /// Статус заявки
    /// </summary>
    [Required(ErrorMessage = "Необходимо указать состояние заявки")]
    public BrigadeStates Status { get; set; }

    /// <summary>
    /// Комментарий к статусу
    /// </summary>
    public string? Comment { get; set; }
}
