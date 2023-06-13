using Cas.SaaS.Contracts.Employee;
using System.ComponentModel.DataAnnotations;

namespace Cas.SaaS.Contracts.Brigade;

public class BrigadeAddDto
{
    /// <summary>
    /// Идентификатор услуги
    /// </summary>
    public Guid ServiceId { get; set; }

    /// <summary>
    /// Идентификаторы сотрудников
    /// </summary>
    [Required, MinLength(1), MaxLength(3)]
    public Guid[] EmployeesId { get; set; } = new Guid[] { };

    /// <summary>
    /// Заказчик ФИО
    /// </summary>
    public string Customer { get; set; } = string.Empty;

    /// <summary>
    /// Номер заказчика
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Адрес заказа
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Комментраии к наряду
    /// </summary>
    public string Description { get; set; } = string.Empty;


    /// <summary>
    /// Дата начала работы
    /// </summary>
    public DateTime StartDate { get; set; }
}

public class BrigadeResultDTO
{
    /// <summary>
    /// Результат
    /// </summary>
    public bool Succeeded { get; set; }

    /// <summary>
    /// Сообщение сервера
    /// </summary>
    public string? Message { get; set; }
}