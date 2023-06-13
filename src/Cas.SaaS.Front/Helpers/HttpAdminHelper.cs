using Cas.SaaS.Client.Services;
using Cas.SaaS.Contracts.Application;
using Cas.SaaS.Contracts.Brigade;
using Cas.SaaS.Contracts.Client;
using Cas.SaaS.Contracts.Delivery;
using Cas.SaaS.Contracts.Employee;
using Cas.SaaS.Contracts.TariffPlan;
using Cas.SaaS.Contracts.User;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using static System.Net.WebRequestMethods;

namespace Cas.SaaS.Client.Helpers;

public class HttpAdminHelper
{
    private readonly ILogger<HttpAdminHelper> _logger;
    private readonly HttpClient _http;
    private readonly ITokenService _tokenService;

    public HttpAdminHelper(ILogger<HttpAdminHelper> logger, HttpClient http, ITokenService tokenService)
    {
        _logger = logger;
        _http = http;
        _tokenService = tokenService;
    }

    /// <summary>
    /// Получить список пользователей
    /// </summary>
    /// <returns></returns>
    public async Task<UserDto[]> GetUsersAsync()
    {
        try
        {
            var token = await _tokenService.GetToken();

            if (token != null && token.Expiration > DateTime.UtcNow)
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", $"{token.AccessToken}");
            }

            return await _http.GetFromJsonAsync<UserDto[]>("api/Users/GetAll");
        }
        catch
        {
            return new UserDto[0];
        }
    }

    /// <summary>
    /// Удалить пользователя по ид
    /// </summary>
    /// <returns></returns>
    public async Task<Guid> DeleteUserById(Guid id)
    {
        try
        {
            var token = await _tokenService.GetToken();

            if (token != null && token.Expiration > DateTime.UtcNow)
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", $"{token.AccessToken}");
            }

            return await _http.GetFromJsonAsync<Guid>($"api/Users/RemoveUser/{id}");
        }
        catch
        {
            return id;
        }
    }

    /// <summary>
    /// Получить список заявок
    /// </summary>
    /// <returns></returns>
    public async Task<ApplicationDto[]> GetApplicationsAsync()
    {
        try
        {
            var token = await _tokenService.GetToken();

            if (token != null && token.Expiration > DateTime.UtcNow)
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", $"{token.AccessToken}");
            }

            return await _http.GetFromJsonAsync<ApplicationDto[]>("api/Applications/GetApplications");
        }
        catch
        {
            return new ApplicationDto[0];
        }
    }

    /// <summary>
    /// Получать число заявок, которые не просмотрели
    /// </summary>
    /// <returns></returns>
    public async Task<ApplicationResultDTO> GetApplicationsIsCheckAsync()
    {
        try
        {
            var token = await _tokenService.GetToken();

            if (token != null && token.Expiration > DateTime.UtcNow)
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", $"{token.AccessToken}");
            }

            var applicationList = await _http.GetFromJsonAsync<ApplicationDto[]>("api/Applications/GetApplications");
            if (applicationList is null) {
                return new ApplicationResultDTO() 
                { 
                    Succeeded = false,
                    Message = "Заявок нет."
                };
            }

            var applicationIsCheck = applicationList.Where(x => x.IsCheck == false).ToList().Count;

            return new ApplicationResultDTO()
            {
                Succeeded = true,
                Message = applicationIsCheck.ToString()
            };
        }
        catch
        {
            return new ApplicationResultDTO
            {
                Succeeded = false,
                Message = "Sorry, we were unable to log you in at this time. Please try again shortly."
            };
        }
    }

    /// <summary>
    /// Добавить клиента
    /// </summary>
    /// <returns></returns>
    public async Task<HttpResponseMessage> AddClientAsync(ClientAddDto clientAddDto)
    {
        try
        {
            var token = await _tokenService.GetToken();

            if (token != null && token.Expiration > DateTime.UtcNow)
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", $"{token.AccessToken}");
            }

            clientAddDto.Login = RandomString(6, true);
            clientAddDto.Password = RandomPassword(10);

            return await _http.PostAsJsonAsync("api/Users/AddClient", clientAddDto);
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Обновить пользователя
    /// </summary>
    /// <returns></returns>
    public async Task<HttpResponseMessage> UpdateUserAsync(UserUpdateDto userUpdateDto)
    {
        try
        {
            var token = await _tokenService.GetToken();

            if (token != null && token.Expiration > DateTime.UtcNow)
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", $"{token.AccessToken}");
            }

            return await _http.PostAsJsonAsync("api/Users/UpdateUser", userUpdateDto);
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Добавить заказ клиента
    /// </summary>
    /// <returns></returns>
    public async Task<HttpResponseMessage> AddDeliveryClientAsync(DeliveryTariffAddDto deliveryTariffAddDto)
    {
        try
        {
            var token = await _tokenService.GetToken();

            if (token != null && token.Expiration > DateTime.UtcNow)
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", $"{token.AccessToken}");
            }

            return await _http.PostAsJsonAsync("api/Users/AddDelivery", deliveryTariffAddDto);
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Детали заявки по идентификатору
    /// </summary>
    /// <returns></returns>
    public async Task<ApplicationDto> ApplicationIsCheckAsync(Guid id)
    {
        try
        {
            var token = await _tokenService.GetToken();

            if (token != null && token.Expiration > DateTime.UtcNow)
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", $"{token.AccessToken}");
            }

            await _http.PostAsJsonAsync($"api/Applications/GetApplication/{id}/check", new ApplicationIsCheckDto());

            return await _http.GetFromJsonAsync<ApplicationDto>($"api/Applications/GetApplication/{id}");
        }
        catch
        {
            return new ApplicationDto();
        }
    }

    /// <summary>
    /// Изменение состояния заявки
    /// </summary>
    /// <param name="applicationAddDto"></param>
    /// <returns></returns>
    public async Task<ApplicationResultDTO> EditApplicationStateAsync(Guid id, ApplicationStatesEditDto applicationStatesEditDto)
    {
        try
        {
            var token = await _tokenService.GetToken();

            if (token != null && token.Expiration > DateTime.UtcNow)
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", $"{token.AccessToken}");
            }

            var response = await _http.PostAsJsonAsync($"api/Applications/GetApplication/{id}/statuses", applicationStatesEditDto);
            var result = await response.Content.ReadFromJsonAsync<ApplicationResultDTO>();
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);

            return new ApplicationResultDTO
            {
                Succeeded = false,
                Message = "Sorry, we were unable to log you in at this time. Please try again shortly."
            };
        }
    }

    /// <summary>
    /// Детали клиента по идентификатору
    /// </summary>
    /// <returns></returns>
    public async Task<UserDetailDto> GetClientByIdAsync(Guid id)
    {
        try
        {
            var token = await _tokenService.GetToken();

            if (token != null && token.Expiration > DateTime.UtcNow)
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", $"{token.AccessToken}");
            }

            return await _http.GetFromJsonAsync<UserDetailDto>($"api/Users/GetUserById/{id}");
        }
        catch
        {
            return new UserDetailDto();
        }
    }

    /// <summary>
    /// Тарифный план по идентификатору
    /// </summary>
    /// <returns></returns>
    public async Task<TariffPlanDto> GetTariffPlanByIdAsync(Guid id)
    {
        try
        {
            var token = await _tokenService.GetToken();

            if (token != null && token.Expiration > DateTime.UtcNow)
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", $"{token.AccessToken}");
            }

            return await _http.GetFromJsonAsync<TariffPlanDto>($"api/Tariffs/GetTariffById/{id}");
        }
        catch
        {
            return new TariffPlanDto();
        }
    }

    /// <summary>
    /// Добавить тарифный план
    /// </summary>
    /// <returns></returns>
    public async Task<HttpResponseMessage> AddTariffPlanAsync(TariffPlanAddDto tariffPlanAddDto)
    {
        try
        {
            var token = await _tokenService.GetToken();

            if (token != null && token.Expiration > DateTime.UtcNow)
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", $"{token.AccessToken}");
            }

            return await _http.PostAsJsonAsync("api/Tariffs/AddTariff", tariffPlanAddDto);
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Обновить тарифный план
    /// </summary>
    /// <returns></returns>
    public async Task<HttpResponseMessage> UpdateTariffPlanAsync(TariffPlanUpdateDto tariffPlanUpdateDto)
    {
        try
        {
            var token = await _tokenService.GetToken();

            if (token != null && token.Expiration > DateTime.UtcNow)
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", $"{token.AccessToken}");
            }

            return await _http.PostAsJsonAsync("api/Tariffs/UpdateTariff", tariffPlanUpdateDto);
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
