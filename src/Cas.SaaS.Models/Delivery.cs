namespace Cas.SaaS.Models;

/// <summary>
/// Заказы клиентов в системе
/// </summary>
public class Delivery
{
    /// <summary>
    /// Идентификатор заказа в системе
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Номер заказа
    /// </summary>
    public string NumberDelivery { get; set; } = string.Empty;

    /// <summary>
    /// Дата оформления услуги
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Дата окончания услуги
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Идентификатор клиента
    /// </summary>
    public Guid ClientId { get; set; }

    /// <summary>
    /// Клиент, который оформил услугу
    /// </summary>
    public virtual Client Client { get; set; } = null!;

    /// <summary>
    /// Идентификатор тариф
    /// </summary>
    public Guid TariffPlanId { get; set; }

    /// <summary>
    /// Тарифный план услуги
    /// </summary>
    public virtual TariffPlan TariffPlan { get; set; } = null!;
}
