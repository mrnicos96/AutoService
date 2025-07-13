using AutoService.Core.DTOs;
using System;

namespace AutoService.Core.Interfaces.Services
{
    public interface IExcelExportService
    {
        byte[] GenerateRepairsReport(ReportDataDto reportData);
        byte[] GenerateMasterWorkloadReport(MasterWorkloadDto reportData);
    }
}