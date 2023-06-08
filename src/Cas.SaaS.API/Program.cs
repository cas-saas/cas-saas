using Cas.SaaS.API;
using Cas.SaaS.API.Helpers;
using Cas.SaaS.API.Options;
using Cas.SaaS.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

ConfigureEmailSender(builder);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{;
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

var jwtOptionsSection = builder.Configuration.GetSection(nameof(JwtOptions));
var jwtOptions = jwtOptionsSection.Get<JwtOptions>();

if (jwtOptions is null)
    throw new Exception("JwtOptions is null");

builder.Services.AddOptions<JwtOptions>().Bind(jwtOptionsSection);
builder.Services.AddScoped(cfg => cfg.GetService<IOptions<JwtOptions>>()?.Value);

builder.Services.AddSingleton<JwtHelper>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts =>
    {
        byte[] signingKeyBytes = Encoding.UTF8
            .GetBytes(jwtOptions.Key);

        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();

void ConfigureEmailSender(WebApplicationBuilder webApplicationBuilder)
{
    var smtpClientOptions = webApplicationBuilder.Configuration.GetSection(nameof(SmtpClientOptions)).Get<SmtpClientOptions>();
    if (smtpClientOptions == null)
    {
        throw new Exception("SmtpClientOptions is null");
    }

    var codeTemplateOptions = webApplicationBuilder.Configuration.GetSection(nameof(CodeTemplateOptions)).Get<CodeTemplateOptions>();
    if (codeTemplateOptions == null)
    {
        throw new Exception("CodeTemplateOptions is null");
    }
    webApplicationBuilder.Services.AddSingleton(new EmailSenderService(smtpClientOptions, codeTemplateOptions));
}