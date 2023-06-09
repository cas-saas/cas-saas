namespace Cas.SaaS.Contracts.Delivery;

public class DeliveryTariffAddDto
{
    /// <summary>
    /// Идентификатор клиента
    /// </summary>
    public string ClientId { get; set; } = string.Empty;

    /// <summary>
    /// Идентификатор тариф
    /// </summary>
    public string TariffPlanId { get; set; } = string.Empty;
}
