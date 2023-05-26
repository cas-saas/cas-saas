using System.ComponentModel.DataAnnotations;

namespace Cas.SaaS.Contracts.Application;

/// <summary>
/// Модель добавления заявки от клиента
/// </summary>
public class ApplicationAddDto
{
    /// <summary>
    /// Название организации
    /// </summary>
    [Required(ErrorMessage = "Необходимо указать название организации к заявке")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Имя клиента
    /// </summary>
    [Required(ErrorMessage = "Необходимо указать ваше имя к заявке")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Комментарий к заявки
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Контакты для связи
    /// </summary>
    [Required(ErrorMessage = "Необходимо указать ваш номер телефона к заявке")]
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Почтовый адрес для связи
    /// </summary>
    [Required(ErrorMessage = "Необходимо указать ваш почтовый адрес к заявке")]
    public string Email { get; set; } = string.Empty;
}
