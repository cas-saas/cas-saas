namespace Cas.SaaS.Contracts.Service;

/// <summary>
/// Модель обновления услуги
/// </summary>
public class ServiceUpdateDto
{
    /// <summary>
    /// Идентификатор услуги
    /// </summary>
    public Guid Id { get; set; }

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
    public List<Tool> Tools { get; set; } = new List<Tool>();
}