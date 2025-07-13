using AutoMapper;
using AutoService.Application.Interfaces;
using AutoService.Core.DTOs;
using AutoService.Core.Interfaces;
using AutoService.Core.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoService.Application.Services
{
    public class ReportService : IReportService_ 
    {
        private readonly IExcelExportService _excelService;
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        public ReportService(
            IServiceScopeFactory scopeFactory,
            IExcelExportService excelService,
            IMapper mapper)
        {
            _scopeFactory = scopeFactory; 
            _excelService = excelService;
            _mapper = mapper;
        }

        public async Task<ReportDataDto> GenerateMonthlyReport(int month, int year)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<IRepairRepository>();
                var repairs = await repository.GetInProgressRepairsForMonth(month, year);

                // Вариант 1: Последовательная обработка (надежнее)
                var repairItems = new List<RepairItemDto>();
                foreach (var repair in repairs)
                {
                    repairItems.Add(new RepairItemDto
                    {
                        RepairStartDate = repair.StartDate,
                        CarMake = repair.Car.Make,
                        CarModel = repair.Car.Model,
                        LicensePlate = repair.Car.LicensePlate,
                        MasterFullName = $"{repair.Master.LastName} {repair.Master.FirstName}",
                        ProblemDescription = repair.ProblemDescription,
                        Cost = repair.WorkCost,
                        MasterWorkloadPercentage = await repository
                            .GetMasterWorkloadPercentage(repair.MasterId, month, year)
                    });
                }

                // Вариант 2: Параллельная обработка (требует thread-safe репозитория)
                // var repairItems = await Task.WhenAll(repairs.Select(async repair => ...));

                return new ReportDataDto
                {
                    Month = month,
                    Year = year,
                    Repairs = repairItems,
                    GrandTotalCost = repairItems.Sum(r => r.Cost)
                };
            }
        }

        public async Task<byte[]> GenerateExcelReport(int month, int year)
        {
            var reportData = await GenerateMonthlyReport(month, year);
            return _excelService.GenerateRepairsReport(reportData);
        }

        public async Task<MasterWorkloadDto> GenerateMasterWorkloadReport(int month, int year)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<IRepairRepository>();
                // Реализация метода...
                throw new NotImplementedException();
            }
        }
    }
}