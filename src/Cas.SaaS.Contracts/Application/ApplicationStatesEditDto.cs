using Cas.SaaS.Models;
using System.ComponentModel.DataAnnotations;

namespace Cas.SaaS.Contracts.Application;

/// <summary>
/// Модель изменения состояния заявки
/// </summary>
public class ApplicationStatesEditDto
{

    /// <summary>
    /// Статус заявки
    /// </summary>
    [Required(ErrorMessage = "Необходимо указать состояние заявки")]
    public ApplicationStates Status { get; set; }

    /// <summary>
    /// Комментарий к статусу
    /// </summary>
    public string? Comment { get; set; }
}
