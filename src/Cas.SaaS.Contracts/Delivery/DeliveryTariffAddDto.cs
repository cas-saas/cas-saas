namespace Cas.SaaS.Contracts.Delivery;

public class DeliveryTariffAddDto
{
    /// <summary>
    /// Идентификатор клиента
    /// </summary>
    public Guid ClientId { get; set; }

    /// <summary>
    /// Идентификатор тариф
    /// </summary>
    public Guid TariffPlanId { get; set; }
}
