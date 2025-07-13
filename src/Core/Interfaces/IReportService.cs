using AutoService.Core.DTOs;

namespace AutoService.Core.Interfaces
{
    public interface IReportService
    {
        Task<ReportDataDto> GenerateMonthlyReport(int month, int year);
        Task<byte[]> GenerateExcelReport(int month, int year);
    }
}