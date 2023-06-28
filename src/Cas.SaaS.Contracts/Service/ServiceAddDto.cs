using System.ComponentModel.DataAnnotations;

namespace Cas.SaaS.Contracts.Service;

/// <summary>
/// Модель добавления услуги
/// </summary>
public class ServiceAddDto
{
    /// <summary>
    /// Название услуги
    /// </summary>
    [Required(ErrorMessage = "Введите название услуги!")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Описание услуги
    /// </summary>
    [Required(ErrorMessage = "Введите описание услуги!")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Необходимые инструменты для оказания услуги
    /// </summary>
    [Required(ErrorMessage = "Добавьте необходимые инструменты для оказания услуги!")]
    public List<Tool> Tools { get; set; } = new List<Tool>();
}