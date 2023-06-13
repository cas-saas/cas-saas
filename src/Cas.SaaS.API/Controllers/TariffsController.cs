using Cas.SaaS.Contracts.Delivery;
using Cas.SaaS.Contracts.Employee;
using Cas.SaaS.Contracts.TariffPlan;
using Cas.SaaS.Contracts.User;
using Cas.SaaS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Cas.SaaS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TariffsController : Controller
{
    private readonly DatabaseContext _context;
    private readonly ILogger<UsersController> _logger;

    /// <summary>
    /// Конструктор класса <see cref="TariffsController" />
    /// </summary>
    /// <param name="context">Контекст базы данных</param>
    /// <param name="logger">Логгер</param>
    public TariffsController(DatabaseContext context, ILogger<UsersController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Получить все тарифы
    /// </summary>
    [Route("GetTariffs"), HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TariffPlanDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetTariffs()
    {
        return Ok(await _context.TariffPlans
            .Select(item => new TariffPlanDto
            {
                Id = item.Id,
                Title = item.Title,
                Payment = item.Payment,
                Price = item.Price,
                Description = item.Description != null ? item.Description : null,
                CountEmployees = item.CountEmployees,
            }).ToListAsync());
    }

    /// <summary>
    /// Добавить тариф в систему
    /// </summary>
    /// <param name="tariffAddDto">Данные по тарифу</param>
    [Authorize(Roles = "Admin")]
    [Route("AddTariff"), HttpPost]
    public async Task<IActionResult> AddTariff([FromBody] TariffPlanAddDto tariffAddDto)
    {
        if (!ModelState.IsValid) return BadRequest();

        var item = new TariffPlan
        {
            Id = Guid.NewGuid(),
            Title = tariffAddDto.Title,
            Payment = tariffAddDto.Payment,
            Price = tariffAddDto.Price,
            Description = tariffAddDto.Description != null ? tariffAddDto.Description : null,
            CountEmployees = tariffAddDto.CountEmployees
        };

        await _context.TariffPlans.AddAsync(item);
        await _context.SaveChangesAsync();
        return Ok(item.Id);
    }

    /// <summary>
    /// Получить тарифный план по его ид
    /// </summary>
    [Route("GetTariffById/{id:guid}"), HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TariffPlanDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetTariffById(Guid id)
    {
        var tariff = await _context.TariffPlans.FirstOrDefaultAsync(x => x.Id == id);
        if (tariff is null)
            return NotFound();

        var tariffPlanDto = new TariffPlanDto
        {
            Id = tariff.Id,
            Title = tariff.Title,
            Payment = tariff.Payment,
            Price = tariff.Price,
            Description = tariff.Description,
            CountEmployees = tariff.CountEmployees
        };

        return Ok(tariffPlanDto);
    }

    /// <summary>
    /// Обновить данные тарифного плана
    /// </summary>
    /// <param name="tariffPlanUpdateDto">Данные по тарифу</param>
    [Route("UpdateTariff"), HttpPost]
    public async Task<IActionResult> UpdateUser([FromBody] TariffPlanUpdateDto tariffPlanUpdateDto)
    {
        if (!ModelState.IsValid) return BadRequest();

        var tariff = await _context.TariffPlans.FirstOrDefaultAsync(item => item.Id == tariffPlanUpdateDto.Id);
        if (tariff is null)
            return BadRequest();

        tariff.Id = tariffPlanUpdateDto.Id;
        tariff.Title = tariffPlanUpdateDto.Title;
        tariff.Payment = tariffPlanUpdateDto.Payment;
        tariff.Price = tariffPlanUpdateDto.Price;
        tariff.Description = tariffPlanUpdateDto.Description;
        tariff.CountEmployees = tariffPlanUpdateDto.CountEmployees;

        _context.TariffPlans.Update(tariff);
        await _context.SaveChangesAsync();
        return Ok(tariff.Id);
    }
}
