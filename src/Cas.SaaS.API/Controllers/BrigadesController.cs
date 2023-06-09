using Cas.SaaS.API.Helpers;
using Cas.SaaS.Contracts.Application;
using Cas.SaaS.Contracts.Brigade;
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
    private readonly JwtHelper _jwtHelper;
    private readonly ILogger<ApplicationsController> _logger;

    /// <summary>
    /// Контроллер класса <see cref="BrigadesController"/>
    /// </summary>
    /// <param name="context">Контекст базы данных</param>
    /// <param name="logger">Логгер</param>
    /// <param name="jwtReader">Расшифровщик данных пользователя из JWT</param>
    /// <exception cref="ArgumentNullException">Аргумент не инициализирован</exception>
    public BrigadesController(DatabaseContext context, JwtHelper jwtHelper, ILogger<ApplicationsController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _jwtHelper = jwtHelper ?? throw new ArgumentNullException(nameof(jwtHelper));
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
                Status = brigade.Status,
                ServiceId = brigade.ServiceId,
                Customer = brigade.Customer,
                Phone = brigade.Phone,
                Address = brigade.Address,
                Description = brigade.Description
            }).ToListAsync());
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

        var brigade = new Brigade
        {
            Id = Guid.NewGuid(),
            Status = BrigadeStates.New,
            ServiceId = brigadeAddDto.ServiceId,
            Service = service,
            Employees = employees,
            Customer = brigadeAddDto.Customer,
            Phone = brigadeAddDto.Phone,
            Address = brigadeAddDto.Address,
            Description = brigadeAddDto.Description
        };

        await _context.Brigades.AddAsync(brigade);
        await _context.SaveChangesAsync();

        return Ok(brigade.Id);
    }
}
