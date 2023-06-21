using System.ComponentModel.DataAnnotations;

namespace Cas.SaaS.Contracts.TariffPlan;

/// <summary>
/// Модель обновления тарифа
/// </summary>
public class TariffPlanUpdateDto
{
    /// <summary>
    /// Идентификатор тарифа
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Название тарифа
    /// </summary>
    [Required(ErrorMessage = "Введите название!")] 
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Период оплаты
    /// </summary>
    [Required(ErrorMessage = "Введите период оплаты!")] 
    public int Payment { get; set; }

    /// <summary>
    /// Цена
    /// </summary>
    [Required(ErrorMessage = "Введите цену!")] 
    public int Price { get; set; }

    /// <summary>
    /// Описание плана
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Количество сотрудников в тарифном плане
    /// </summary>
    [Required(ErrorMessage = "Введите количество сотрудников!")] 
    public int CountEmployees { get; set; }
}
