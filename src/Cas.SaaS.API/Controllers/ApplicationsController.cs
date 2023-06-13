using Cas.SaaS.API.Services;
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
    private readonly EmailSenderService _emailSenderService;
    private readonly ILogger<ApplicationsController> _logger;

    /// <summary>
    /// Контроллер класса <see cref="ApplicationsController"/>
    /// </summary>
    /// <param name="context">Контекст базы данных</param>
    /// <param name="logger">Логгер</param>
    /// <param name="jwtReader">Расшифровщик данных пользователя из JWT</param>
    /// <exception cref="ArgumentNullException">Аргумент не инициализирован</exception>
    public ApplicationsController(DatabaseContext context, EmailSenderService emailSenderService, ILogger<ApplicationsController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _emailSenderService = emailSenderService ?? throw new ArgumentNullException(nameof(emailSenderService));
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

        Random rnd = new Random();
        string genNumber = string.Format("AP-{0}-{1}-{2}", DateTime.UtcNow.Day + rnd.Next(10000), DateTime.UtcNow.Month, DateTime.UtcNow.Year);

        var application = new Application
        {
            Id = Guid.NewGuid(),
            NumberApplication = genNumber,
            Title = applicationAddDto.Title,
            Name = applicationAddDto.Name,
            Description = applicationAddDto.Description,
            Phone = applicationAddDto.Phone,
            Email = applicationAddDto.Email,
            CreatedDate = DateTime.UtcNow,
            Status = ApplicationStates.New,
            IsCheck = false
        };

        await _emailSenderService.SendStatus(application);

        await _context.Applications.AddAsync(application);
        await _context.SaveChangesAsync();

        var resultApplication = new ApplicationResultDTO
        {
            Succeeded = true,
            Message = $"Заявка №{application.Id} успешно создана!"
        };

        return Ok(resultApplication);
    }

    /// <summary>
    /// Получить заявку по идентификатору
    /// </summary>
    /// <param name="state">Состояние заявки</param>
    /// <response code="200">Список всех заявок</response>
    /// <response code="401">Токен доступа истек</response>
    /// <response code="500">Ошибка сервера</response>
    [Route("GetApplication/{id:guid}"), HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApplicationDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetApplication(Guid id)
    {
        var application = await _context.Applications.FirstOrDefaultAsync(x => x.Id == id);
        if (application is null)
            return BadRequest();

        var applicationDto = new ApplicationDto
        {
            Id = application.Id,
            NumberApplication = application.NumberApplication,
            Title = application.Title,
            Name = application.Name,
            Description = application.Description,
            Phone = application.Phone,
            Email = application.Email,
            CreatedDate = application.CreatedDate,
            Status = application.Status,
            IsCheck = application.IsCheck
        };
        return Ok(applicationDto);
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
                NumberApplication = application.NumberApplication,
                Title = application.Title,
                Name = application.Name,
                Description = application.Description,
                Phone = application.Phone,
                Email = application.Email,
                CreatedDate = application.CreatedDate,
                Status = application.Status,
                IsCheck = application.IsCheck
            }).OrderBy(x => x.IsCheck).ToListAsync());
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
    [HttpPost("GetApplication/{id:guid}/statuses")]
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

        await _emailSenderService.SendEditStatus(application);

        _context.Applications.Update(application);
        await _context.SaveChangesAsync();

        var resultApplication = new ApplicationResultDTO
        {
            Succeeded = true,
            Message = $"Статус заявки №{application.Id} успешно изменён!"
        };

        return Ok(resultApplication);
    }

    /// <summary>
    /// Изменить статус просмотра заявки
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <param name="applicationIsCheckDto">Модель изменения состояния</param>
    /// <returns></returns>
    [HttpPost("GetApplication/{id:guid}/check")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ApplicationIsCheck([FromRoute] Guid id, [FromBody] ApplicationIsCheckDto applicationIsCheckDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(applicationIsCheckDto);

        var application = await _context.Applications
            .FirstOrDefaultAsync(item => item.Id == id);
        if (application is null)
            return NotFound();

        application.IsCheck = applicationIsCheckDto.IsCheck;

        _context.Applications.Update(application);
        await _context.SaveChangesAsync();
        return Ok(application);
    }

    #endregion
}
