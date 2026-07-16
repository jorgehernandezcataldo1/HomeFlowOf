using FluentValidation;
using AutoMapper;
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

            // Specific validators
            services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
            services.AddScoped<IValidator<ChangePasswordRequest>, ChangePasswordRequestValidator>();
            services.AddScoped<IValidator<RegisterCorredorRequest>, RegisterCorredorRequestValidator>();
            services.AddScoped<IValidator<CreateClienteRequest>, CreateClienteRequestValidator>();
            services.AddScoped<IValidator<CreatePropietarioRequest>, CreatePropietarioRequestValidator>();
            services.AddScoped<IValidator<CreateArrendatarioRequest>, CreateArrendatarioRequestValidator>();
            services.AddScoped<IValidator<CreatePropiedadRequest>, CreatePropiedadRequestValidator>();
            services.AddScoped<IValidator<BuscarPropiedadRequest>, BuscarPropiedadRequestValidator>();
            services.AddScoped<IValidator<CreateOrdenVisitaRequest>, CreateOrdenVisitaRequestValidator>();
            services.AddScoped<IValidator<CreateChecklistArrendatarioRequest>, CreateChecklistArrendatarioRequestValidator>();
            services.AddScoped<IValidator<CreateContratoArriendoRequest>, CreateContratoArriendoRequestValidator>();

            // Registrar servicios
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IPropertyService, PropertyService>();

            // Registrar AutoMapper
            services.AddAutoMapper(typeof(ApplicationExtensions));

            return services;
        }
    }
}
