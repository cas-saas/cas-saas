using Cas.SaaS.Client.Services;
using Cas.SaaS.Contracts.Application;
using Cas.SaaS.Contracts.Employee;
using Cas.SaaS.Contracts.User;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Cas.SaaS.Client.Helpers;

public class HttpAuthHelper
{
    private readonly ILogger<HttpAuthHelper> _logger;
    private readonly HttpClient _http;
    private readonly ITokenService _tokenService;
    private readonly CustomAuthenticationStateProvider _myAuthenticationStateProvider;

    public HttpAuthHelper(ILogger<HttpAuthHelper> logger, HttpClient http, ITokenService tokenService, CustomAuthenticationStateProvider myAuthenticationStateProvider)
    {
        _logger = logger;
        _http = http;
        _tokenService = tokenService;
        _myAuthenticationStateProvider = myAuthenticationStateProvider;
    }

    /// <summary>
    /// Вход в систему
    /// </summary>
    /// <param name="userLoginDTO"></param>
    /// <returns></returns>
    public async Task<UserLoginResultDTO> LoginUser(UserLoginDto userLoginDTO)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("api/Auth", userLoginDTO);
            var result = await response.Content.ReadFromJsonAsync<UserLoginResultDTO>();
            await _tokenService.SetToken(result.Token);
            _myAuthenticationStateProvider.StateChanged();
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);

            return new UserLoginResultDTO
            {
                Succeeded = false,
                Message = "Sorry, we were unable to log you in at this time. Please try again shortly."
            };
        }
    }

    /// <summary>
    /// Выход пользователя
    /// </summary>
    /// <returns></returns>
    public async Task LogoutUser()
    {
        await _tokenService.RemoveToken();
        _myAuthenticationStateProvider.StateChanged();
    }

    /// <summary>
    /// Отправка заявки в систему
    /// </summary>
    /// <param name="applicationAddDto"></param>
    /// <returns></returns>
    public async Task<ApplicationResultDTO> AddApplicationAsync(ApplicationAddDto applicationAddDto)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("api/Applications/AddApplication", applicationAddDto);
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
    /// Детали заявки по идентификатору
    /// </summary>
    /// <returns></returns>
    public async Task<ApplicationDto> GetApplicationByIdAsync(Guid id)
    {
        try
        {
            var token = await _tokenService.GetToken();

            if (token != null && token.Expiration > DateTime.UtcNow)
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", $"{token.AccessToken}");
            }

            return await _http.GetFromJsonAsync<ApplicationDto>($"api/Applications/GetApplications/{id}");
        }
        catch
        {
            return new ApplicationDto();
        }
    }
}
