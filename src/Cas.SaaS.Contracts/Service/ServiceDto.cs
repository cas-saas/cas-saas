namespace Cas.SaaS.Contracts.Service;

/// <summary>
/// Модель вывода услуги
/// </summary>
public class ServiceDto
{
    /// <summary>
    /// Идентификатор услуги
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Название услуги
    /// </summary>
    public string Name { get; set; } = string.Empty;
}