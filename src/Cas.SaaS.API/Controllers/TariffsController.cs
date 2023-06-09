using Cas.SaaS.Contracts.TariffPlan;
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
}
