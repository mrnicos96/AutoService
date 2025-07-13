using AutoService.Core.Interfaces;
using AutoService.Core.Interfaces.Services;
using AutoService.Infrastructure.Data;
using AutoService.Infrastructure.Repositories;
using AutoService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AutoService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Database
            services.AddDbContext<AutoServiceDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("AutoServiceConnection"),
                    b => b.MigrationsAssembly(typeof(AutoServiceDbContext).Assembly.FullName)));

            // Repositories
            services.AddScoped<IRepairRepository, RepairRepository>();

            // Services
            services.AddScoped<IExcelExportService, ExcelExportService>();

            return services;
        }
    }
}