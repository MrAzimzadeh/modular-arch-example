using Application;
using Core.Attributes;
using Identity;
using Infrastructure.EF;
using Infrastructure.EF.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi;

[DependsOn(
    typeof(ApplicationModule),
    typeof(InfrastructureEFModule),
    typeof(IdentityModule)
)]
public class WebApiModule
{
    public static IServiceCollection AddWebApiModule(IServiceCollection services, IConfiguration configuration)
    {
        // Add all dependent modules
        services.AddApplicationModule();
        services.AddInfrastructureEFModule(configuration);
        services.AddIdentityModule<AppDbContext>(configuration);
        
        // Add Controllers
        services.AddControllers();
        
        // Add Authorization
        services.AddAuthorization();
        
        // Add OpenAPI
        services.AddOpenApi();
        
        return services;
    }
    
    public static async Task<IServiceProvider> InitializeWebApiModuleAsync(IServiceProvider serviceProvider)
    {
        // Seed Identity Data
        await IdentityModule.SeedIdentityDataAsync(serviceProvider);
        
        return serviceProvider;
    }
}

// Extension methods for easier usage
public static class WebApiModuleExtensions
{
    public static IServiceCollection AddWebApiModule(this IServiceCollection services, IConfiguration configuration)
    {
        return WebApiModule.AddWebApiModule(services, configuration);
    }

    public static async Task<IServiceProvider> InitializeWebApiModuleAsync(this IServiceProvider serviceProvider)
    {
        return await WebApiModule.InitializeWebApiModuleAsync(serviceProvider);
    }
}
