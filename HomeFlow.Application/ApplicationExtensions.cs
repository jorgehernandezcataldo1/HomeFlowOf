using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using HomeFlow.Application.Interfaces;
using HomeFlow.Application.Services;
using HomeFlow.Application.Validators;
using HomeFlow.Shared.DTOs.Auth;
using HomeFlow.Shared.DTOs.Clients;
using HomeFlow.Shared.DTOs.Properties;
using HomeFlow.Shared.DTOs.Processes;

namespace HomeFlow.Application
{
    /// <summary>
    /// Extensiones para registrar servicios de la capa Application
    /// </summary>
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Registrar validadores
            services.AddValidatorsFromAssemblyContaining(typeof(ApplicationExtensions));

            // Servicios de aplicacion.
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
