using Cas.SaaS.API.Helpers;
using Cas.SaaS.Contracts.Application;
using Cas.SaaS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Cas.SaaS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApplicationsController : Controller
{
    private readonly DatabaseContext _context;
    private readonly JwtHelper _jwtHelper;
    private readonly ILogger<ApplicationsController> _logger;

    /// <summary>
    /// Контроллер класса <see cref="ApplicationsController"/>
    /// </summary>
    /// <param name="context">Контекст базы данных</param>
    /// <param name="logger">Логгер</param>
    /// <param name="jwtReader">Расшифровщик данных пользователя из JWT</param>
    /// <exception cref="ArgumentNullException">Аргумент не инициализирован</exception>
    public ApplicationsController(DatabaseContext context, JwtHelper jwtHelper, ILogger<ApplicationsController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _jwtHelper = jwtHelper ?? throw new ArgumentNullException(nameof(jwtHelper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    #region ActionClient

    /// <summary>
    /// Добавить заявку
    /// </summary>
    /// <param name="applicationAddDto">Данные по заявке</param>
    /// <response code="200">Заявка успешно добавлена</response>
    /// <response code="400">Переданны некорректные данные</response>
    /// <response code="500">Ошибка сервера</response>
    [Route("AddApplication"), HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddApplication([FromBody] ApplicationAddDto applicationAddDto)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var application = new Application
        {
            Id = Guid.NewGuid(),
            Title = applicationAddDto.Title,
            Name = applicationAddDto.Name,
            Description = applicationAddDto.Description,
            Phone = applicationAddDto.Phone,
            Email = applicationAddDto.Email,
            CreatedDate = DateTime.UtcNow,
            Status = ApplicationStates.New
        };

        await _context.Applications.AddAsync(application);
        await _context.SaveChangesAsync();

        return Ok(application.Id);
    }

    #endregion

    #region ActionAdmin

    /// <summary>
    /// Получить заявки
    /// </summary>
    /// <param name="state">Состояние заявки</param>
    /// <response code="200">Список всех заявок</response>
    /// <response code="401">Токен доступа истек</response>
    /// <response code="500">Ошибка сервера</response>
    [Route("GetApplications"), HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ApplicationDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetApplications()
    {
        return Ok(await _context.Applications
            .Select(application => new ApplicationDto
            {
                Id = application.Id,
                Title = application.Title,
                Name = application.Name,
                Description = application.Description,
                Phone = application.Phone,
                Email = application.Email,
                CreatedDate = application.CreatedDate,
                Status = application.Status
            }).ToListAsync());
    }

    /// <summary>
    /// Получить заявку по идентификатору
    /// </summary>
    /// <param name="state">Состояние заявки</param>
    /// <response code="200">Список всех заявок</response>
    /// <response code="401">Токен доступа истек</response>
    /// <response code="500">Ошибка сервера</response>
    [Route("GetApplications/{id:guid}"), HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ApplicationDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetApplications(Guid id)
    {
        return Ok(await _context.Applications.Where(item => item.Id == id)
            .Select(application => new ApplicationDto
            {
                Id = application.Id,
                Title = application.Title,
                Name = application.Name,
                Description = application.Description,
                Phone = application.Phone,
                Email = application.Email,
                CreatedDate = application.CreatedDate,
                Status = application.Status
            }).ToListAsync());
    }

    /// <summary>
    /// Изменить состояние заявки
    /// </summary>
    /// <param name="id">Идентификатор заявки</param>
    /// <param name="applicationStatesEditDto">Данные по состоянию</param>
    /// <response code="204">Статус к заявке успешно добавлен</response>
    /// <response code="400">Переданны некорректные данные</response>
    /// <response code="401">Токен доступа истек</response>
    /// <response code="404">Заявка не найдена</response>
    /// <response code="409">Ошибка в статусе заявки</response>
    /// <response code="500">Ошибка сервера</response>
    [HttpPost("GetApplications/{id:guid}/statuses")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EditApplicationStates([FromRoute] Guid id, [FromBody] ApplicationStatesEditDto applicationStatesEditDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(applicationStatesEditDto);

        var application = await _context.Applications
            .FirstOrDefaultAsync(item => item.Id == id);
        if (application is null)
            return NotFound();

        var currentState = application.Status;
        if (currentState == ApplicationStates.Rejected)
            return Conflict("Невозможно изменить статус заявки, потому что она была отклонена!");

        application.Status = applicationStatesEditDto.Status;

        _context.Applications.Update(application);
        await _context.SaveChangesAsync();
        return Ok(application);
    }

    #endregion
}
