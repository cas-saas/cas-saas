namespace Cas.SaaS.Contracts.Service;

/// <summary>
/// Модель добавления услуги
/// </summary>
public class ServiceAddDto
{
    /// <summary>
    /// Название услуги
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Описание услуги
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// Необходимые инструменты для оказания услуги
    /// </summary>
    public List<string> Tools { get; set; } = null!;
    /// <summary>
    /// Идентификатор клиента
    /// </summary>
    public Guid ClientId { get; set; }
}