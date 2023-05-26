namespace Cas.SaaS.Models;

/// <summary>
/// Модель услуг услуги
/// </summary>
public class Service
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
    public List<string> Tools { get; set; } = null!;
    /// <summary>
    /// Идентификатор клиента
    /// </summary>
    public Guid ClientId { get; set; }
    /// <summary>
    /// Клиент
    /// </summary>
    public virtual Client Client { get; set; } = null!;
    /// <summary>
    /// Наряды, к которым привязана услуга
    /// </summary>
    public virtual IEnumerable<Brigade> Brigades { get; set; } = null!;
}
