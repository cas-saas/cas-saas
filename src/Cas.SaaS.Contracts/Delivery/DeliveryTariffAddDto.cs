using System.ComponentModel.DataAnnotations;

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
    [Required(ErrorMessage = "Выберите тарифный план!")] 
    public string TariffPlanId { get; set; } = string.Empty;
}
