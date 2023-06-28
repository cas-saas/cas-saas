using Cas.SaaS.Contracts.Employee;
using System.ComponentModel.DataAnnotations;

namespace Cas.SaaS.Contracts.Brigade;

public class BrigadeAddDto
{
    /// <summary>
    /// Идентификатор услуги
    /// </summary>
    [Required(ErrorMessage = "Выберите оказываемую услугу!")]
    public Guid ServiceId { get; set; }

    /// <summary>
    /// Идентификаторы сотрудников
    /// </summary>
    [Required, MinLength(1, ErrorMessage = "Укажите хотя бы одного сотрудника!")]
    public Guid[] EmployeesId { get; set; } = new Guid[] { };

    /// <summary>
    /// Заказчик ФИО
    /// </summary>
    [Required(ErrorMessage = "Введите ФИО заказчика!")]
    public string Customer { get; set; } = string.Empty;

    /// <summary>
    /// Номер заказчика
    /// </summary>
    [Required(ErrorMessage = "Введите номер заказчика!")]
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Адрес заказа
    /// </summary>
    [Required(ErrorMessage = "Введите адрес заказа!")]
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Комментраии к наряду
    /// </summary>
    [Required(ErrorMessage = "Напишите комментраии к наряду!")]
    public string Description { get; set; } = string.Empty;


    /// <summary>
    /// Дата начала работы
    /// </summary>
    [Required(ErrorMessage = "Укажите дату начала работы!")]
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
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