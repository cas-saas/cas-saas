using Cas.SaaS.Models;

namespace Cas.SaaS.Contracts.Application;

/// <summary>
/// Модель вывода заявки от клиента
/// </summary>
public class ApplicationDto
{
    /// <summary>
    /// Идентификатор заявки
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Название организации
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Имя клиента
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Комментарий к заявки
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Контакты для связи
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Почтовый адрес для связи
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Дата оформления заявки
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Статус заявки
    /// </summary>
    public ApplicationStates Status { get; set; }
}
