using AutoService.Core.DTOs;
using System.Threading.Tasks;

namespace AutoService.Application.Interfaces
{
    public interface IReportService_
    {
        Task<ReportDataDto> GenerateMonthlyReport(int month, int year);
        Task<MasterWorkloadDto> GenerateMasterWorkloadReport(int month, int year);
        Task<byte[]> GenerateExcelReport(int month, int year);
    }
}