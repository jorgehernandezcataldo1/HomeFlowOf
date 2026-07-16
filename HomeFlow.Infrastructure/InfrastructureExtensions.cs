using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HomeFlow.Domain.Interfaces;
using HomeFlow.Infrastructure.Repositories;
using HomeFlow.Infrastructure.Data;

namespace HomeFlow.Infrastructure
{
    /// <summary>
    /// Extensiones de servicios para inyección de dependencias en Infrastructure
    /// </summary>
    public static class InfrastructureExtensions
    {
        /// <summary>
        /// Añade los servicios de infraestructura necesarios
        /// </summary>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString), "La cadena de conexión no puede estar vacía.");

            // Registrar DbContext
            services.AddDbContext<HomeFlowDbContext>(options =>
            {
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly("HomeFlow.Infrastructure");
                    sqlOptions.CommandTimeout(30);
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelaySeconds: 10, errorNumbersToAdd: null);
                });
            });

            // Registrar servicios
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        /// <summary>
        /// Aplica migraciones pendientes en la base de datos
        /// </summary>
        public static void ApplyMigrations(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<HomeFlowDbContext>();

                try
                {
                    // Aplicar migraciones automáticamente
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al aplicar migraciones: {ex.Message}");
                    // Log error pero no fallar la aplicación
                }
            }
        }

        /// <summary>
        /// Inicializa la base de datos con datos de prueba si es necesario
        /// </summary>
        public static async Task InitializeDatabaseAsync(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<HomeFlowDbContext>();

                // Aplicar migraciones
                await context.Database.MigrateAsync();
            }
        }
    }
}
