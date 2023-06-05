using Cas.SaaS.Models;
using Cas.SaaS.Contracts.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Cas.SaaS.Contracts.Client;
using Cas.SaaS.Contracts.Employee;
using Cas.SaaS.Contracts.TariffPlan;
using Cas.SaaS.Contracts.Delivery;

namespace Cas.SaaS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class UsersController : Controller
{
    private readonly DatabaseContext _context;
    private readonly ILogger<UsersController> _logger;

    /// <summary>
    /// Конструктор класса <see cref="UsersController" />
    /// </summary>
    /// <param name="context">Контекст базы данных</param>
    /// <param name="logger">Логгер</param>
    public UsersController(DatabaseContext context, ILogger<UsersController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Получить всех пользователей
    /// </summary>
    [Route("GetAll"), HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _context.Users
            .Select(item => new UserDto
            {
                Id = item.Id,
                Login = item.Login,
                FIO = $"{item.Name} {item.Surname} {item.Patronymic}",
                Phone = item.Phone,
                Email = item.Email,
                Role = item.Role,
            }).ToListAsync());
    }

    /// <summary>
    /// Получить всех пользователей с ролью "Клиент"
    /// </summary>
    [Route("GetClients"), HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ClientDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetClients()
    {
        return Ok(await _context.Clients
            .Select(item => new ClientDto
            {
                Id = item.Id,
                Login = item.Login,
                Phone = item.Phone,
                Email = item.Email,
                Name = item.Name,
                Surname = item.Surname,
                Patronymic = item.Patronymic,
                Role = item.Role,
                Status = item.Status
            }).ToListAsync());
    }

    /// <summary>
    /// Получить всех сотрудников с клиента по его ид
    /// </summary>
    [Route("GetEmployeesById/{id:guid}"), HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<EmployeeDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetEmployeesById(Guid id)
    {
        return Ok(await _context.Employees.Where(x => x.ClientId == id)
            .Select(item => new EmployeeDto
            {
                Id = item.Id,
                Login = item.Login,
                Phone = item.Phone,
                Email = item.Email,
                Name = item.Name,
                Surname = item.Surname,
                Patronymic = item.Patronymic,
                Role = item.Role,
                IsActive = item.IsActive
            }).ToListAsync());
    }

    /// <summary>
    /// Добавить клиента в систему
    /// </summary>
    /// <param name="clientAddDto">Данные по клиенту</param>
    [Route("AddClient"), HttpPost]
    public async Task<IActionResult> AddClient([FromBody] ClientAddDto clientAddDto)
    {
        if (!ModelState.IsValid) return BadRequest();

        var client = await _context.Clients.FirstOrDefaultAsync(item => item.Login == clientAddDto.Login);
        if (client is not null)
        {
            client.Phone = clientAddDto.Phone;
            client.Email = clientAddDto.Email;
            client.Name = clientAddDto.Name;
            client.Surname = clientAddDto.Surname;
            client.Patronymic = clientAddDto.Patronymic != null ? clientAddDto.Patronymic : null;

            _context.Clients.Update(client);
            await _context.SaveChangesAsync();
            return Ok(client);
        }

        var item = new Cas.SaaS.Models.Client
        {
            Id = Guid.NewGuid(),
            Email = clientAddDto.Email,
            Phone = clientAddDto.Phone,
            Login = clientAddDto.Login,
            Password = clientAddDto.Password,
            Name = clientAddDto.Name,
            Surname = clientAddDto.Surname,
            Patronymic = clientAddDto.Patronymic != null ? clientAddDto.Patronymic : null,
            Role = UserRoles.Client,
        };

        await _context.Clients.AddAsync(item);
        await _context.SaveChangesAsync();
        return Ok(item);
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
        return Ok(item);
    }

    /// <summary>
    /// Получить все заказы
    /// </summary>
    [Route("GetDeliveries"), HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<DeliveryDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetDeliveries()
    {
        return Ok(await _context.Deliveries
            .Select(item => new DeliveryDto
            {
                Id = item.Id,
                CreatedDate = item.CreatedDate,
                EndDate = item.EndDate,
                ClientId = item.ClientId,
                TariffPlanId = item.TariffPlanId
            }).ToListAsync());
    }

    /// <summary>
    /// Добавление заказа в систему
    /// </summary>
    /// <param name="deliveryTariffAddDto">Данные по заказу и выбранному тарифу</param>
    /// <returns></returns>
    [Route("AddDelivery"), HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddDelivery([FromBody] DeliveryTariffAddDto deliveryTariffAddDto)
    {
        if (!ModelState.IsValid) return BadRequest();

        var tariff = await _context.TariffPlans.FindAsync(deliveryTariffAddDto.TariffPlanId);
        if (tariff is null)
            return BadRequest();

        var client = await _context.Clients.FindAsync(deliveryTariffAddDto.ClientId);
        if (client is null)
            return BadRequest();

        var item = new Delivery
        {
            Id = Guid.NewGuid(),
            CreatedDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(tariff.Payment),
            ClientId = deliveryTariffAddDto.ClientId,
            Client = client,
            TariffPlanId = deliveryTariffAddDto.TariffPlanId,
            TariffPlan = tariff
        };

        await _context.Deliveries.AddAsync(item);
        await _context.SaveChangesAsync();
        return Ok(item);
    }
}
