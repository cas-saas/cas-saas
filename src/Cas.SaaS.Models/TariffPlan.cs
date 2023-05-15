namespace Cas.SaaS.Models;

/// <summary>
/// Тарифный план услуги
/// </summary>
public class TariffPlan
{
    /// <summary>
    /// Идентификатор тарифа
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Название тарифа
    /// </summary>
    public string Title { get; set; } = string.Empty;
    /// <summary>
    /// Период оплаты
    /// </summary>
    public int Payment { get; set; }
    /// <summary>
    /// Цена
    /// </summary>
    public int Price { get; set; }
    /// <summary>
    /// Описание плана
    /// </summary>
    public string? Description { get; set; }
    /// <summary>
    /// Количество сотрудников в тарифном плане
    /// </summary>
    public int CountEmployees { get; set; }
    /// <summary>
    /// Заказ
    /// </summary>
    public virtual Delivery Delivery { get; set; } = null!;
}
