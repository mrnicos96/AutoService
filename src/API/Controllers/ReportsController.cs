
using AutoService.Application.Interfaces;
using AutoService.Core.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace AutoService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService_ _reportService;

        public ReportsController(IReportService_ reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("monthly")]
        public async Task<ActionResult<ReportDataDto>> GetMonthlyReport(
            [FromQuery] int month,
            [FromQuery] int year)
        {
            var report = await _reportService.GenerateMonthlyReport(month, year);
            return Ok(report);
        }

        [HttpGet("monthly/csv")]
        public async Task<IActionResult> GetMonthlyReportCsv(
            [FromQuery] int month,
            [FromQuery] int year)
        {
            var report = await _reportService.GenerateMonthlyReport(month, year);

            // Формируем CSV
            var csv = new StringBuilder();
            csv.AppendLine("Дата,Мастер,Марка,Модель,Госномер,Проблема,Стоимость,% загрузки");

            foreach (var repair in report.Repairs)
            {
                csv.AppendLine($"\"{repair.RepairStartDate:dd.MM.yyyy}\"," +
                              $"\"{repair.MasterFullName}\"," +
                              $"\"{repair.CarMake}\"," +
                              $"\"{repair.CarModel}\"," +
                              $"\"{repair.LicensePlate}\"," +
                              $"\"{repair.ProblemDescription}\"," +
                              $"{((int)repair.Cost).ToString()}," +
                              $"{((int)repair.MasterWorkloadPercentage).ToString()}");
            }

            return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", $"report_{month}_{year}.csv");
        }

        [HttpGet("monthly/excel")]
        public async Task<IActionResult> GetMonthlyReportExcel(
            [FromQuery] int month,
            [FromQuery] int year)
        {
            var excelData = await _reportService.GenerateExcelReport(month, year);
            return File(excelData,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"RepairsReport_{month}_{year}.xlsx");
        }
    }
}