using Cas.SaaS.Client.Services;
using Cas.SaaS.Contracts.User;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace Cas.SaaS.Client.Helpers;

public class HttpClientHelper
{
    private readonly ILogger<HttpClientHelper> _logger;
    private readonly HttpClient _http;
    private readonly ITokenService _tokenService;
    private readonly CustomAuthenticationStateProvider _myAuthenticationStateProvider;

    public HttpClientHelper(ILogger<HttpClientHelper> logger, HttpClient http, ITokenService tokenService, CustomAuthenticationStateProvider myAuthenticationStateProvider)
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
}
