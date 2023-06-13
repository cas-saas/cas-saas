using Cas.SaaS.Client.Services;
using Cas.SaaS.Contracts.Application;
using Cas.SaaS.Contracts.Brigade;
using Cas.SaaS.Contracts.Client;
using Cas.SaaS.Contracts.Employee;
using Cas.SaaS.Contracts.Service;
using Cas.SaaS.Contracts.User;
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
    /// Получить список услуг
    /// </summary>
    /// <returns></returns>
    public async Task<ServiceDto[]> GetServicesAsync()
    {
        try
        {
            var token = await _tokenService.GetToken();

            if (token != null && token.Expiration > DateTime.UtcNow)
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", $"{token.AccessToken}");
            }

            return await _http.GetFromJsonAsync<ServiceDto[]>("api/Clients/GetServices");
        }
        catch
        {
            return new ServiceDto[0];
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

            return await _http.PostAsJsonAsync("api/Clients/AddEmployee", employeeDto);
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Обновить данные сотрудника
    /// </summary>
    /// <returns></returns>
    public async Task<HttpResponseMessage> UpdateEmployeeAsync(EmployeeUpdateDto employeeDto)
    {
        try
        {
            var token = await _tokenService.GetToken();

            if (token != null && token.Expiration > DateTime.UtcNow)
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", $"{token.AccessToken}");
            }

            return await _http.PostAsJsonAsync("api/Clients/UpdateEmployee", employeeDto);
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Детали сотрудника по идентификатору
    /// </summary>
    /// <returns></returns>
    public async Task<EmployeeDetailDto> GetEmployeeByIdAsync(Guid id)
    {
        try
        {
            var token = await _tokenService.GetToken();

            if (token != null && token.Expiration > DateTime.UtcNow)
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", $"{token.AccessToken}");
            }

            return await _http.GetFromJsonAsync<EmployeeDetailDto>($"api/Clients/GetEmployeeById/{id}");
        }
        catch
        {
            return new EmployeeDetailDto();
        }
    }

    /// <summary>
    /// Удалить сотрудника по ид
    /// </summary>
    /// <returns></returns>
    public async Task<Guid> DeleteEmployeeById(Guid id)
    {
        try
        {
            var token = await _tokenService.GetToken();

            if (token != null && token.Expiration > DateTime.UtcNow)
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", $"{token.AccessToken}");
            }

            return await _http.GetFromJsonAsync<Guid>($"api/Clients/RemoveEmployee/{id}");
        }
        catch
        {
            return id;
        }
    }

    /// <summary>
    /// Удалить услугу по ид
    /// </summary>
    /// <returns></returns>
    public async Task<Guid> DeleteServiceById(Guid id)
    {
        try
        {
            var token = await _tokenService.GetToken();

            if (token != null && token.Expiration > DateTime.UtcNow)
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", $"{token.AccessToken}");
            }

            return await _http.GetFromJsonAsync<Guid>($"api/Clients/RemoveService/{id}");
        }
        catch
        {
            return id;
        }
    }

    /// <summary>
    /// Добавить услугу
    /// </summary>
    /// <returns></returns>
    public async Task<HttpResponseMessage> AddServiceAsync(ServiceAddDto serviceAddDto)
    {
        try
        {
            var token = await _tokenService.GetToken();

            if (token != null && token.Expiration > DateTime.UtcNow)
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", $"{token.AccessToken}");
            }

            return await _http.PostAsJsonAsync("api/Clients/AddService", serviceAddDto);
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Обновить данные услуги
    /// </summary>
    /// <returns></returns>
    public async Task<HttpResponseMessage> UpdateServiceAsync(ServiceUpdateDto serviceUpdateDto)
    {
        try
        {
            var token = await _tokenService.GetToken();

            if (token != null && token.Expiration > DateTime.UtcNow)
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", $"{token.AccessToken}");
            }

            return await _http.PostAsJsonAsync("api/Clients/UpdateService", serviceUpdateDto);
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Детали сотрудника по идентификатору
    /// </summary>
    /// <returns></returns>
    public async Task<ServiceDto> GetServiceByIdAsync(Guid id)
    {
        try
        {
            var token = await _tokenService.GetToken();

            if (token != null && token.Expiration > DateTime.UtcNow)
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", $"{token.AccessToken}");
            }

            return await _http.GetFromJsonAsync<ServiceDto>($"api/Clients/GetServiceById/{id}");
        }
        catch
        {
            return new ServiceDto();
        }
    }

    /// <summary>
    /// Добавить наряд
    /// </summary>
    /// <returns></returns>
    public async Task<HttpResponseMessage> AddBrigadeAsync(BrigadeAddDto brigadeAddDto)
    {
        try
        {
            var token = await _tokenService.GetToken();

            if (token != null && token.Expiration > DateTime.UtcNow)
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", $"{token.AccessToken}");
            }

            return await _http.PostAsJsonAsync("api/Brigades/AddBrigade", brigadeAddDto);
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Детали наряда по идентификатору
    /// </summary>
    /// <returns></returns>
    public async Task<BrigadeDetailDto> GetBrigadeByIdAsync(Guid id)
    {
        try
        {
            var token = await _tokenService.GetToken();

            if (token != null && token.Expiration > DateTime.UtcNow)
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", $"{token.AccessToken}");
            }

            return await _http.GetFromJsonAsync<BrigadeDetailDto>($"api/Brigades/GetBrigadeDetail/{id}");
        }
        catch
        {
            return new BrigadeDetailDto();
        }
    }

    /// <summary>
    /// Изменение состояния наряда
    /// </summary>
    /// <param name="brigadeStatesEditDto"></param>
    /// <returns></returns>
    public async Task<BrigadeResultDTO> EditBrigadeStateAsync(Guid id, BrigadeStatesEditDto brigadeStatesEditDto)
    {
        try
        {
            var token = await _tokenService.GetToken();

            if (token != null && token.Expiration > DateTime.UtcNow)
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", $"{token.AccessToken}");
            }

            var response = await _http.PostAsJsonAsync($"api/Brigades/GetBrigade/{id}/statuses", brigadeStatesEditDto);
            var result = await response.Content.ReadFromJsonAsync<BrigadeResultDTO>();
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);

            return new BrigadeResultDTO
            {
                Succeeded = false,
                Message = "Sorry, we were unable to log you in at this time. Please try again shortly."
            };
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
