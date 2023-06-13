using Cas.SaaS.Contracts.Employee;
using Cas.SaaS.Contracts.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using Cas.SaaS.API.Helpers;
using Cas.SaaS.Models;
using Cas.SaaS.API.Services;

namespace Cas.SaaS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin, Client")]
public class ClientsController : Controller
{
    private readonly DatabaseContext _context;
    private readonly EmailSenderService _emailSenderService;
    private readonly JwtHelper _jwtHelper;
    private readonly ILogger<UsersController> _logger;

    /// <summary>
    /// Конструктор класса <see cref="UsersController" />
    /// </summary>
    /// <param name="context">Контекст базы данных</param>
    /// <param name="logger">Логгер</param>
    public ClientsController(DatabaseContext context, EmailSenderService emailSenderService, JwtHelper jwtHelper, ILogger<UsersController> logger)
    {
        _context = context;
        _emailSenderService = emailSenderService;
        _jwtHelper = jwtHelper;
        _logger = logger;
    }

    /// <summary>
    /// Получить всех сотрудников клиента
    /// </summary>
    [Route("GetEmployees"), HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmployeeDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetEmployees()
    {
        var clientInfo = GetAuthUserInfo();
        if (clientInfo is null)
            return Unauthorized();

        return Ok(await _context.Employees.Where(x => x.ClientId == clientInfo.GuidId)
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
    /// Добавить сотрудника в систему
    /// </summary>
    /// <param name="employeeAddDto">Данные по сотруднику</param>
    [Route("AddEmployee"), HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddEmployee([FromBody] EmployeeAddDto employeeAddDto)
    {
        var clientInfo = GetAuthUserInfo();
        if (clientInfo is null)
            return Unauthorized();

        if (!ModelState.IsValid) return BadRequest();

        var delivery = await _context.Deliveries.FirstOrDefaultAsync(item => item.ClientId == clientInfo.GuidId);
        if (delivery is null)
            return Conflict("Ошибка! Заказ на использование небыл оформлен!");

        var client = await _context.Clients.FindAsync(clientInfo.GuidId);
        if (client is null)
            return BadRequest();

        if (client.Status == ClientStatus.NotPaid)
            return Conflict("Ошибка! Аккаунт не был оплачен!");

        var currentPlan = await _context.TariffPlans.FirstOrDefaultAsync(item => item.Id == delivery.TariffPlanId);
        if (currentPlan is null) 
            return NoContent();

        var employeeList = GetListEmployee(clientInfo.GuidId);

        if (employeeList.Count() >= currentPlan.CountEmployees)
            return Conflict("Ошибка! Первышен лимит добавления сотрудников!");

        var item = new Employee
        {
            Id = Guid.NewGuid(),
            Email = employeeAddDto.Email,
            Phone = employeeAddDto.Phone,
            Login = employeeAddDto.Login,
            Password = employeeAddDto.Password,
            Name = employeeAddDto.Name,
            Surname = employeeAddDto.Surname,
            Patronymic = employeeAddDto.Patronymic,
            Role = UserRoles.Employee,
            ClientId = clientInfo.GuidId,
            Client = client,
            IsActive = false
        };

        await _emailSenderService.SendEmployee(item);

        await _context.Employees.AddAsync(item);
        await _context.SaveChangesAsync();
        return Ok();
    }

    /// <summary>
    /// Удалить пользователя
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Route("RemoveEmployee/{id:guid}"), HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveUser(Guid id)
    {
        var employee = await _context.Employees.FirstOrDefaultAsync(item => item.Id == id);
        if (employee is null)
            return BadRequest();

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    /// <summary>
    /// Обновить сотрудника в системе
    /// </summary>
    /// <param name="employeeAddDto">Данные по сотруднику</param>
    [Route("UpdateEmployee"), HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateEmployee([FromBody] EmployeeUpdateDto employeeUpdateDto)
    {
        var clientInfo = GetAuthUserInfo();
        if (clientInfo is null)
            return Unauthorized();

        if (!ModelState.IsValid) return BadRequest();

        var delivery = await _context.Deliveries.FirstOrDefaultAsync(item => item.ClientId == clientInfo.GuidId);
        if (delivery is null)
            return Conflict("Ошибка! Заказ на использование небыл оформлен!");

        var client = await _context.Clients.FindAsync(clientInfo.GuidId);
        if (client is null)
            return BadRequest();

        if (client.Status == ClientStatus.NotPaid)
            return Conflict("Ошибка! Аккаунт не был оплачен!");

        var employee = await _context.Employees.FirstOrDefaultAsync(item => item.Id == employeeUpdateDto.Id);
        if (employee is not null)
        {
            employee.Phone = employeeUpdateDto.Phone;
            employee.Email = employeeUpdateDto.Email;
            employee.Name = employeeUpdateDto.Name;
            employee.Surname = employeeUpdateDto.Surname;
            employee.Patronymic = employeeUpdateDto.Patronymic;

            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
            return Ok();
        }

        return BadRequest();
    }

    /// <summary>
    /// Получить сотрудника по его ид
    /// </summary>
    [Route("GetEmployeeById/{id:guid}"), HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmployeeDetailDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetEmployeeById(Guid id)
    {
        var employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
        if (employee is null)
            return NotFound();

        var employeeDetailDto = new EmployeeDetailDto
        {
            Id = employee.Id,
            Email = employee.Email,
            Phone = employee.Phone,
            Name = employee.Name,
            Surname = employee.Surname,
            Patronymic = employee.Patronymic,
            IsActive = false
        };

        return Ok(employeeDetailDto);
    }

    /// <summary>
    /// Получить все услуги клиента
    /// </summary>
    [Route("GetServices"), HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetServices()
    {
        var clientInfo = GetAuthUserInfo();
        if (clientInfo is null)
            return Unauthorized();

        return Ok(await _context.Services.Where(x => x.ClientId == clientInfo.GuidId)
            .Select(item => new ServiceDto
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Tools = item.Tools
            }).ToListAsync());
    }

    /// <summary>
    /// Добавить услугу в систему
    /// </summary>
    /// <param name="serviceAddDto">Данные по услуге</param>
    [Route("AddService"), HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddService([FromBody] ServiceAddDto serviceAddDto)
    {
        var clientInfo = GetAuthUserInfo();
        if (clientInfo is null)
            return Unauthorized();

        if (!ModelState.IsValid) return BadRequest();

        var client = await _context.Clients.FindAsync(clientInfo.GuidId);
        if (client is null)
            return BadRequest();

        var tools = serviceAddDto.Tools.Select(x => x.Name).ToList();

        var item = new Service
        {
            Id = Guid.NewGuid(),
            Name = serviceAddDto.Name,
            Description = serviceAddDto.Description,
            Tools = tools,
            ClientId = clientInfo.GuidId,
            Client = client
        };

        await _context.Services.AddAsync(item);
        await _context.SaveChangesAsync();
        return Ok();
    }

    /// <summary>
    /// Удалить пользователя
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Route("RemoveService/{id:guid}"), HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveService(Guid id)
    {
        var service = await _context.Services.FirstOrDefaultAsync(item => item.Id == id);
        if (service is null)
            return BadRequest();

        _context.Services.Remove(service);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    /// <summary>
    /// Обновить услугу в системе
    /// </summary>
    /// <param name="employeeAddDto">Данные по услуге</param>
    [Route("UpdateService"), HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateService([FromBody] ServiceUpdateDto serviceUpdateDto)
    {
        var clientInfo = GetAuthUserInfo();
        if (clientInfo is null)
            return Unauthorized();

        if (!ModelState.IsValid) return BadRequest();

        var service = await _context.Services.FirstOrDefaultAsync(item => item.Id == serviceUpdateDto.Id);
        if (service is not null)
        {
            service.Name = serviceUpdateDto.Name;
            service.Description = serviceUpdateDto.Description;
            service.Tools = serviceUpdateDto.Tools.Select(x => x.Name).ToList();

            _context.Services.Update(service);
            await _context.SaveChangesAsync();
            return Ok();
        }

        return BadRequest();
    }

    /// <summary>
    /// Получить сотрудника по его ид
    /// </summary>
    [Route("GetServiceById/{id:guid}"), HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetServiceById(Guid id)
    {
        var service = await _context.Services.FirstOrDefaultAsync(x => x.Id == id);
        if (service is null)
            return NotFound();

        var serviceDto = new ServiceDto
        {
            Id = service.Id,
            Name = service.Name,
            Tools = service.Tools,
            Description = service.Description
        };

        return Ok(serviceDto);
    }

    /*    [Route("GetClient"), HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ClientDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetClient()
        {
            var clientInfo = GetAuthUserInfo();
            if (clientInfo is null)
                return Unauthorized();

            var client = await _context.Clients.FirstOrDefaultAsync(x => x.Id == clientInfo.GuidId);
            if (client is null)
                return NotFound();

            var clientDto = new ClientDto()
            {
                Id = client.Id,
                Login = client.Login,
                Phone = client.Phone,
                Email = client.Email,
                Name = client.Name,
                Surname = client.Surname,
                Patronymic = client.Patronymic,
                Role = client.Role,
                Status = client.Status
            };

            return Ok(clientDto);
        }*/

    #region Claims

    private AuthUserInfo? GetAuthUserInfo()
    {
        string? authHeader = Request.Headers["Authorization"];
        var token = authHeader?.Replace("Bearer ", "") ?? throw new ArgumentNullException($"Bearer token not found");

        _ = _jwtHelper.ReadAccessToken(token, out var claims, out var validTo);
        if (claims is null) return null;

        var userInfo = new AuthUserInfo(
            Id: claims.Claims.FirstOrDefault(a => a.Type == ClaimsIdentity.DefaultIssuer)?.Value ?? throw new ArgumentNullException($"User's id from bearer token not found"),
            FullName: claims.Claims.FirstOrDefault(a => a.Type == ClaimsIdentity.DefaultNameClaimType)?.Value ?? throw new ArgumentNullException($"User's fullname from bearer token not found"),
            Role: claims.Claims.FirstOrDefault(a => a.Type == ClaimsIdentity.DefaultRoleClaimType)?.Value ?? throw new ArgumentNullException($"User's role from bearer token not found"),
            Email: claims.Claims.FirstOrDefault(a => a.Type == ClaimTypes.Email)?.Value ?? throw new ArgumentNullException($"User's email from bearer token not found"),
            Phone: claims.Claims.FirstOrDefault(a => a.Type == ClaimTypes.MobilePhone)?.Value ?? throw new ArgumentNullException($"User's phone from bearer token not found")
        );

        return userInfo;
    }

    private record AuthUserInfo(string Id, string FullName, string Role, string Email, string Phone)
    {
        public Guid GuidId => Guid.TryParse(Id, out var guidId) ? guidId : throw new ArgumentNullException();
    }

    #endregion

    #region Func

    private List<EmployeeDto> GetListEmployee(Guid clientId)
    {
        return _context.Employees.Where(x => x.ClientId == clientId)
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
            }).ToList();
    }

    #endregion
}
