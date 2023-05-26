namespace Cas.SaaS.Contracts.Delivery;

/// <summary>
/// Модель вывода заказа
/// </summary>
public class DeliveryDto
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
    /// Идентификатор клиента
    /// </summary>
    public Guid ClientId { get; set; }
    /// <summary>
    /// Идентификатор тариф
    /// </summary>
    public Guid TariffPlanId { get; set; }
}
