using AutoService.Application.Interfaces;

using AutoService.Application.Mappings;
using AutoService.Application.Services;
using AutoService.Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AutoService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Сервисы
            services.AddScoped<IReportService_, ReportService>();

            // AutoMapper
            services.AddAutoMapper(typeof(ReportProfile));

            // Валидаторы
            services.AddValidatorsFromAssemblyContaining<ReportRequestValidator>();

            return services;
        }
    }
}