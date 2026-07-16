using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using HomeFlow.Infrastructure.Persistence;
using HomeFlow.Infrastructure.Repositories;

namespace HomeFlow.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string? connectionString)
    {
        // DbContext
        services.AddDbContext<HomeFlowDbContext>(options =>
            options.UseSqlServer(connectionString ?? "Server=(localdb)\\mssqllocaldb;Database=HomeFlowDb;Trusted_Connection=true;")
        );

        // Repositorios
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<IPropiedadRepository, PropiedadRepository>();

        return services;
    }
}
