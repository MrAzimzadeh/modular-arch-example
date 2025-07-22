// ✅ DOĞRU Dapper Module
using Core.Attributes;
using Domain;
using Domain.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Dapper;

[DependsOn(typeof(DomainModule))]
public class InfrastructureDapperModule
{
    public static IServiceCollection AddInfrastructureDapperModule(
        IServiceCollection services, 
        IConfiguration configuration)
    {
        // ✅ Domain'e referans - Interface'leri implement eder
        services.AddDomainModule();
        
        // ✅ Repository implementations
        services.AddScoped<IBookRepository, DapperBookRepository>();
        
        return services;
    }
}

// ❌ YANLIŞ - Application'a referans alırsa
// [DependsOn(typeof(ApplicationModule))] // BU YANLIŞ!
