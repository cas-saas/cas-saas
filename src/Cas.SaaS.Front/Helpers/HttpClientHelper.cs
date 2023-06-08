using Cas.SaaS.Client.Services;
using Cas.SaaS.Contracts.Brigade;
using Cas.SaaS.Contracts.Client;
using Cas.SaaS.Contracts.Employee;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace Cas.SaaS.Client.Helpers;

public class HttpClientHelper
{
    private readonly ILogger<HttpClientHelper> _logger;
    private readonly HttpClient _http;
    private readonly ITokenService _tokenService;

    public HttpClientHelper(ILogger<HttpClientHelper> logger, HttpClient http, ITokenService tokenService)
    {
        _logger = logger;
        _http = http;
        _tokenService = tokenService;
    }

    /// <summary>
    /// Получить список сотрудников
    /// </summary>
    /// <returns></returns>
    public async Task<EmployeeDto[]> GetEmployeesAsync()
    {
        try
        {
            var token = await _tokenService.GetToken();

            if (token != null && token.Expiration > DateTime.UtcNow)
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", $"{token.AccessToken}");
            }

            return await _http.GetFromJsonAsync<EmployeeDto[]>("api/Clients/GetEmployees");
        }
        catch
        {
            return new EmployeeDto[0];
        }
    }

    /// <summary>
    /// Получить список нарядов
    /// </summary>
    /// <returns></returns>
    public async Task<BrigadeDto[]> GetBrigadesAsync()
    {
        try
        {
            var token = await _tokenService.GetToken();

            if (token != null && token.Expiration > DateTime.UtcNow)
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", $"{token.AccessToken}");
            }

            return await _http.GetFromJsonAsync<BrigadeDto[]>("api/Brigades/GetBrigades");
        }
        catch
        {
            return new BrigadeDto[0];
        }
    }

    public async Task<ClientDto> GetClientInfo()
    {
        try
        {
            var token = await _tokenService.GetToken();

            if (token != null && token.Expiration > DateTime.UtcNow)
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", $"{token.AccessToken}");
            }

            return await _http.GetFromJsonAsync<ClientDto>("api/Clients/GetClient");
        }
        catch
        {
            return new ClientDto();
        }
    }

    /// <summary>
    /// Добавить сотрудника
    /// </summary>
    /// <returns></returns>
    public async Task<HttpResponseMessage> AddEmployeeAsync(EmployeeAddDto employeeDto)
    {
        try
        {
            var token = await _tokenService.GetToken();

            if (token != null && token.Expiration > DateTime.UtcNow)
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", $"{token.AccessToken}");
            }

            employeeDto.Login = RandomString(6, true);
            employeeDto.Password = RandomPassword(10);

            return await _http.PostAsJsonAsync("api/Clients/AddEmployees", employeeDto);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public string RandomString(int size, bool lowerCase)
    {
        StringBuilder builder = new StringBuilder();
        Random random = new Random();
        char ch;
        for (int i = 0; i < size; i++)
        {
            ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
            builder.Append(ch);
        }
        if (lowerCase)
            return builder.ToString().ToLower();
        return builder.ToString();
    }

    public string RandomPassword(int size = 0)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(RandomString(4, true));
        builder.Append(RandomString(2, false));
        return builder.ToString();
    }
}
