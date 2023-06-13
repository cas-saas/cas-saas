using Cas.SaaS.Models;
using Cas.SaaS.Contracts.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Cas.SaaS.Contracts.Client;
using Cas.SaaS.Contracts.Employee;
using Cas.SaaS.Contracts.Delivery;
using Cas.SaaS.API.Services;

namespace Cas.SaaS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class UsersController : Controller
{
    private readonly DatabaseContext _context;
    private readonly EmailSenderService _emailSenderService;
    private readonly ILogger<UsersController> _logger;

    /// <summary>
    /// Конструктор класса <see cref="UsersController" />
    /// </summary>
    /// <param name="context">Контекст базы данных</param>
    /// <param name="logger">Логгер</param>
    public UsersController(DatabaseContext context, EmailSenderService emailSenderService, ILogger<UsersController> logger)
    {
        _context = context;
        _emailSenderService = emailSenderService;
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
        return Ok(await _context.Users.Where(item => item.Role != UserRoles.Admin)
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
    [Route("GetEmployeesByClientId/{id:guid}"), HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<EmployeeDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetEmployeesByClientId(Guid id)
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
    /// Получить пользователя по его ид
    /// </summary>
    [Route("GetUserById/{id:guid}"), HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDetailDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (user is null)
            return NotFound();

        var client = await _context.Clients.FirstOrDefaultAsync(x => x.Id == user.Id);
        if (client is null)
            client = new Models.Client();

        var employee = await _context.Employees.Where(x => x.ClientId == user.Id).
            Select(item => new EmployeeDto
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
            }).ToListAsync();
        if (employee.Count == 0)
            employee = new List<EmployeeDto>();

        var deliveries = await _context.Deliveries.Where(x => x.ClientId == user.Id).
            Select(item => new DeliveryDto
            {
                Id = item.Id,
                NumberDelivery = item.NumberDelivery,
                CreatedDate = item.CreatedDate,
                EndDate = item.EndDate,
                ClientId = item.ClientId,
                TariffPlanId = item.TariffPlanId
            }).ToListAsync();
        if (deliveries.Count == 0)
            deliveries = new List<DeliveryDto>();

        var clientDetailDto = new UserDetailDto
        {
            Id = user.Id,
            Login = user.Login,
            Phone = user.Phone,
            Email = user.Email,
            Name = user.Name,
            Surname = user.Surname,
            Patronymic = user.Patronymic,
            Role = user.Role,
            Status = client.Status,
            Employees = employee,
            Deliveries = deliveries
        };

        return Ok(clientDetailDto);
    }

    /// <summary>
    /// Добавить клиента в систему
    /// </summary>
    /// <param name="clientAddDto">Данные по клиенту</param>
    [Route("AddClient"), HttpPost]
    public async Task<IActionResult> AddClient([FromBody] ClientAddDto clientAddDto)
    {
        if (!ModelState.IsValid) return BadRequest();

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
            Status = ClientStatus.NotPaid,
            Role = UserRoles.Client,
        };

        await _emailSenderService.SendData(item);

        await _context.Clients.AddAsync(item);
        await _context.SaveChangesAsync();
        return Ok(item.Id);
    }

    /// <summary>
    /// Обновить данные пользователя
    /// </summary>
    /// <param name="userUpdateDto">Данные по клиенту</param>
    [Route("UpdateUser"), HttpPost]
    public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDto userUpdateDto)
    {
        if (!ModelState.IsValid) return BadRequest();

        var user = await _context.Users.FirstOrDefaultAsync(item => item.Id == userUpdateDto.Id);
        if (user is null)
            return BadRequest();

        user.Name = userUpdateDto.Name;
        user.Surname = userUpdateDto.Surname;
        user.Patronymic = userUpdateDto.Patronymic;
        user.Phone = userUpdateDto.Phone;
        user.Email = userUpdateDto.Email;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return Ok(user.Id);
    }

    /// <summary>
    /// Удалить пользователя
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Route("RemoveUser/{id:guid}"), HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveUser(Guid id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(item => item.Id == id);
        if (user is null)
            return BadRequest();

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
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
                NumberDelivery = item.NumberDelivery,
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

        var tariff = await _context.TariffPlans.FindAsync(Guid.Parse(deliveryTariffAddDto.TariffPlanId));
        if (tariff is null)
            return BadRequest();

        var client = await _context.Clients.FindAsync(Guid.Parse(deliveryTariffAddDto.ClientId));
        if (client is null)
            return BadRequest();

        client.Status = ClientStatus.Paid;
        _context.Clients.Update(client);

        Random rnd = new Random();
        string genNumber = string.Format("DR-{0}-{1}-{2}", DateTime.UtcNow.Day + rnd.Next(10000), DateTime.UtcNow.Month, DateTime.UtcNow.Year);

        var item = new Delivery
        {
            Id = Guid.NewGuid(),
            NumberDelivery = genNumber,
            CreatedDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(tariff.Payment),
            ClientId = Guid.Parse(deliveryTariffAddDto.ClientId),
            Client = client,
            TariffPlanId = Guid.Parse(deliveryTariffAddDto.TariffPlanId),
            TariffPlan = tariff
        };

        await _context.Deliveries.AddAsync(item);
        await _context.SaveChangesAsync();
        return Ok(item.Id);
    }
}
