using Cas.SaaS.Contracts.Employee;
using Cas.SaaS.Contracts.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using Cas.SaaS.API.Helpers;
using Cas.SaaS.Models;

namespace Cas.SaaS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin, Client")]
public class ClientsController : Controller
{
    private readonly DatabaseContext _context;
    private readonly JwtHelper _jwtHelper;
    private readonly ILogger<UsersController> _logger;

    /// <summary>
    /// Конструктор класса <see cref="UsersController" />
    /// </summary>
    /// <param name="context">Контекст базы данных</param>
    /// <param name="logger">Логгер</param>
    public ClientsController(DatabaseContext context, JwtHelper jwtHelper, ILogger<UsersController> logger)
    {
        _context = context;
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
    [Route("AddEmployees"), HttpPost]
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
            return NoContent();

        var currentPlan = await _context.TariffPlans.FirstOrDefaultAsync(item => item.Id == delivery.TariffPlanId);
        if (currentPlan is null) 
            return NoContent();

        var employeeList = GetListEmployee(clientInfo.GuidId);

        if (employeeList.Count() >= currentPlan.CountEmployees)
            return Conflict("Ошибка! Первышен лимит добавления сотрудников!");

        var employee = await _context.Employees.FirstOrDefaultAsync(item => item.Login == employeeAddDto.Login);
        if (employee is not null)
        {
            employee.Phone = employeeAddDto.Phone;
            employee.Email = employeeAddDto.Email;
            employee.Name = employeeAddDto.Name;
            employee.Surname = employeeAddDto.Surname;
            employee.Patronymic = employeeAddDto.Patronymic != null ? employeeAddDto.Patronymic : null;

            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
            return Ok(employee);
        }

        var client = await _context.Clients.FindAsync(clientInfo.GuidId);
        if (client is null)
            return BadRequest();

        var item = new Employee
        {
            Id = Guid.NewGuid(),
            Email = employeeAddDto.Email,
            Phone = employeeAddDto.Phone,
            Login = employeeAddDto.Login,
            Password = employeeAddDto.Password,
            Name = employeeAddDto.Name,
            Surname = employeeAddDto.Surname,
            Patronymic = employeeAddDto.Patronymic != null ? employeeAddDto.Patronymic : null,
            Role = UserRoles.Employee,
            ClientId = clientInfo.GuidId,
            Client = client,
            IsActive = false,
        };

        await _context.Employees.AddAsync(item);
        await _context.SaveChangesAsync();
        return Ok(item);
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

        var item = new Service
        {
            Id = Guid.NewGuid(),
            Name = serviceAddDto.Name,
            Description = serviceAddDto.Description,
            Tools = serviceAddDto.Tools,
            ClientId = clientInfo.GuidId,
            Client = client
        };

        await _context.Services.AddAsync(item);
        await _context.SaveChangesAsync();
        return Ok(item);
    }

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
