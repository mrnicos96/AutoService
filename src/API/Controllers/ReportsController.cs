
using AutoService.Application.Interfaces;
using AutoService.Core.DTOs;
using Microsoft.AspNetCore.Mvc;

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