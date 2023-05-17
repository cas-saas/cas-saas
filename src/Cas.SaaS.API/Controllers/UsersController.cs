using Cas.SaaS.Models;
using Cas.SaaS.Contracts.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Cas.SaaS.Contracts.Client;

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
    [HttpGet]
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
    /// Добавить клиента в систему
    /// </summary>
    /// <param name="userAddDto">Данные по клиенту</param>
    [HttpPost]
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
            client.Status = clientAddDto.Status;

            _context.Clients.Update(client);
            await _context.SaveChangesAsync();
            return Ok(client);
        }

        var item = new Client
        {
            Id = Guid.NewGuid(),
            Email = clientAddDto.Email,
            Phone = clientAddDto.Phone,
            Login = clientAddDto.Login,
            Password = clientAddDto.Password,
            Name = clientAddDto.Name,
            Surname = clientAddDto.Surname,
            Patronymic = clientAddDto.Patronymic != null ? clientAddDto.Patronymic : null,
            Role = clientAddDto.Role,
        };

        await _context.Clients.AddAsync(item);
        await _context.SaveChangesAsync();
        return Ok(item);
    }
}
