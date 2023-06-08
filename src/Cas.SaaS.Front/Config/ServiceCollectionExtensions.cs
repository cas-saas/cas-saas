namespace Cas.SaaS.Client.Config;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPolicies(this IServiceCollection services)
    {
        services.AddAuthorizationCore(config =>
        {
            config.AddPolicy("Client", policy => policy.RequireClaim("role", "Client"));
            config.AddPolicy("Admin", policy => policy.RequireClaim("role", "Admin"));
        });

        return services;
    }
}
