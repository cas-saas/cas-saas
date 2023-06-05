using Cas.SaaS.Client;
using Cas.SaaS.Client.Helpers;
using Cas.SaaS.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using Cas.SaaS.Client.Config;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddHttpClient<HttpClientHelper>(client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

// builder.Services.AddAuthorizationCore();
builder.Services.AddPolicies();

builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthenticationStateProvider>());

builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
