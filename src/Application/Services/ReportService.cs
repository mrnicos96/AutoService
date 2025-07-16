using AutoMapper;
using AutoService.Application.Interfaces;
using AutoService.Core.DTOs;
using AutoService.Core.Entities;
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

        //public async Task<ReportDataDto> GenerateVBAReport(int month, int year)
        //{
        //    using (var scope = _scopeFactory.CreateScope())
        //    {
        //        var repository = scope.ServiceProvider.GetRequiredService<IRepairRepository>();
        //        var repairs = await repository.GetInProgressRepairsForMonth(month, year);

        //        // Дополнительная сортировка
        //        var sortedRepairs = repairs
        //            .OrderBy(r => r.StartDate)
        //            .ThenBy(r => r.Master.FullName)
        //            .ThenBy(r => r.Car.Make)
        //            .ToList();

        //        var reportData = new ReportDataDto
        //        {
        //            Month = month,
        //            Year = year,
        //            Repairs = sortedRepairs.Select(r => new RepairItemDto
        //            {
        //                RepairStartDate = r.StartDate,  
        //                CarMake = r.Car.Make,
        //                CarModel = r.Car.Model,
        //                LicensePlate = r.Car.LicensePlate,
        //                MasterFullName = r.Master.FullName,
        //                ProblemDescription = r.ProblemDescription,
        //                Cost = r.WorkCost,
        //                MasterWorkloadPercentage = await repository
        //                    .GetMasterWorkloadPercentage(r.MasterId, month, year)
        //            }),
        //            GrandTotalCost = sortedRepairs.Sum(r => r.WorkCost)
        //        };

        //        return reportData;
        //    }
        //}
    }
}