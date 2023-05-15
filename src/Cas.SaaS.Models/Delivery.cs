namespace Cas.SaaS.Models;

/// <summary>
/// Заявки клиентов в системе
/// </summary>
public class Delivery
{
    /// <summary>
    /// Идентификатор заявки в системе
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Дата оформления услуги
    /// </summary>
    public DateTime CreatedDate { get; set; }
    /// <summary>
    /// Дата окончания услуги
    /// </summary>
    public DateTime EndDate { get; set; }
    /// <summary>
    /// Клиент, который оформил услугу
    /// </summary>
    public virtual Client Client { get; set; } = null!;
    /// <summary>
    /// Тарифный план услуги
    /// </summary>
    public virtual TariffPlan TariffPlan { get; set; } = null!;
}
