using Cas.SaaS.Contracts.Application;
using Cas.SaaS.Contracts.Brigade;
using Cas.SaaS.Contracts.Employee;
using Cas.SaaS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Cas.SaaS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Client, Employee")]
public class BrigadesController : Controller
{
    private readonly DatabaseContext _context;
    private readonly ILogger<ApplicationsController> _logger;

    /// <summary>
    /// Контроллер класса <see cref="BrigadesController"/>
    /// </summary>
    /// <param name="context">Контекст базы данных</param>
    /// <param name="logger">Логгер</param>
    /// <param name="jwtReader">Расшифровщик данных пользователя из JWT</param>
    /// <exception cref="ArgumentNullException">Аргумент не инициализирован</exception>
    public BrigadesController(DatabaseContext context, ILogger<ApplicationsController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Получить заявки
    /// </summary>
    /// <response code="200">Список всех заявок</response>
    /// <response code="401">Токен доступа истек</response>
    /// <response code="500">Ошибка сервера</response>
    [Route("GetBrigades"), HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BrigadeDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBrigades()
    {
        return Ok(await _context.Brigades
            .Select(brigade => new BrigadeDto
            {
                Id = brigade.Id,
                NumberBrigade = brigade.NumberBrigade,
                Status = brigade.Status,
                StartDate = brigade.StartDate,
                EndDate = brigade.EndDate,
                CreatedDate = brigade.CreatedDate
            }).ToListAsync());
    }

    /// <summary>
    /// Получить детали наряда
    /// </summary>
    /// <response code="200">Список всех заявок</response>
    /// <response code="401">Токен доступа истек</response>
    /// <response code="500">Ошибка сервера</response>
    [Route("GetBrigadeDetail/{id:guid}"), HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BrigadeDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBrigadeDetail(Guid id)
    {
        var brigade = await _context.Brigades.Include(b => b.Employees).FirstOrDefaultAsync(item => item.Id == id);
        if (brigade is null)
            return NotFound("Наряд не найден!");

        var brigadeDetail = new BrigadeDetailDto
        {
            Id = brigade.Id,
            NumberBrigade = brigade.NumberBrigade,
            Status = brigade.Status,
            ServiceId = brigade.ServiceId,
            Customer = brigade.Customer,
            Phone = brigade.Phone,
            Address = brigade.Address,
            Description = brigade.Description,
            StartDate = brigade.StartDate,
            EndDate = brigade.EndDate,
            CreatedDate = brigade.CreatedDate,
            EmployeesId = brigade.Employees.Select(item => new EmployeeDetailDto
            {
                Id = item.Id,
                Name = item.Name,
                Patronymic = item.Patronymic,
                Surname = item.Surname,
                Email = item.Email,
                Phone = item.Phone,
                IsActive = item.IsActive
            }).ToList()
        };

        return Ok(brigadeDetail);
    }

    /// <summary>
    /// Добавить наряд
    /// </summary>
    /// <param name="requestAddDto">Данные по наряду</param>
    /// <response code="200">Заявка успешно добавлена</response>
    /// <response code="400">Переданны некорректные данные</response>
    /// <response code="500">Ошибка сервера</response>
    [Route("AddBrigade"), HttpPost]
    [Authorize(Roles = "Client")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddBrigade([FromBody] BrigadeAddDto brigadeAddDto)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var service = await _context.Services.FirstOrDefaultAsync(item => item.Id == brigadeAddDto.ServiceId);
        if (service is null)
            return NoContent();

        var employees = _context.Employees.Where(item => brigadeAddDto.EmployeesId.Any(id => id == item.Id)).ToList();
        if (employees is null)
            return NoContent();

        Random rnd = new Random();
        string genNumber = string.Format("BG-{0}-{1}-{2}", DateTime.UtcNow.Day + rnd.Next(10000), DateTime.UtcNow.Month, DateTime.UtcNow.Year);

        var brigade = new Brigade
        {
            Id = Guid.NewGuid(),
            NumberBrigade = genNumber,
            Status = BrigadeStates.New,
            ServiceId = brigadeAddDto.ServiceId,
            Service = service,
            Employees = employees,
            Customer = brigadeAddDto.Customer,
            Phone = brigadeAddDto.Phone,
            Address = brigadeAddDto.Address,
            Description = brigadeAddDto.Description,
            StartDate = brigadeAddDto.StartDate,
            CreatedDate = DateTime.UtcNow,
        };

        await _context.Brigades.AddAsync(brigade);


        foreach (var employee in employees)
        {
            var brigades = employee.Brigades.ToList();

            if (brigades is null)
                brigades = new List<Brigade> { brigade };
            else
                brigades.Add(brigade);

            employee.Brigades = brigades;
            _context.Employees.Update(employee);
        }

        await _context.SaveChangesAsync();

        return Ok(brigade.Id);
    }

    /// <summary>
    /// Изменить состояние наряда
    /// </summary>
    /// <param name="id">Идентификатор наряда</param>
    /// <param name="applicationStatesEditDto">Данные по состоянию</param>
    /// <response code="204">Статус к заявке успешно добавлен</response>
    /// <response code="400">Переданны некорректные данные</response>
    /// <response code="401">Токен доступа истек</response>
    /// <response code="404">Заявка не найдена</response>
    /// <response code="409">Ошибка в статусе заявки</response>
    /// <response code="500">Ошибка сервера</response>
    [HttpPost("GetBrigade/{id:guid}/statuses")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EditBrigadeStates([FromRoute] Guid id, [FromBody] BrigadeStatesEditDto brigadeStatesEditDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(brigadeStatesEditDto);

        var brigade = await _context.Brigades
            .FirstOrDefaultAsync(item => item.Id == id);
        if (brigade is null)
            return NotFound();

        var currentState = brigade.Status;
        if (currentState == BrigadeStates.RejectedByEmployee || currentState == BrigadeStates.CanceledByClient)
            return Conflict("Невозможно изменить статус заявки, потому что она была отклонена!");

        if (currentState == brigadeStatesEditDto.Status)
            return Conflict("Невозможно изменить статус заявки на тот же!");

        if (brigadeStatesEditDto.Status == BrigadeStates.InProgress)
        {
            brigade.Status = brigadeStatesEditDto.Status;
            brigade.StartDate = DateTime.UtcNow;
            brigade.EndDate = DateTime.MinValue;
        }
        else
        {
            brigade.Status = brigadeStatesEditDto.Status;
            brigade.EndDate = DateTime.UtcNow;
        }

        _context.Brigades.Update(brigade);
        await _context.SaveChangesAsync();

        var resultApplication = new ApplicationResultDTO
        {
            Succeeded = true,
            Message = $"Статус заявки №{brigade.Id} успешно изменён!"
        };

        return Ok(resultApplication);
    }

}
