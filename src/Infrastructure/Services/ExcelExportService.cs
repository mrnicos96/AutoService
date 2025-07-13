using AutoService.Core.DTOs;
using AutoService.Core.Interfaces.Services;
using OfficeOpenXml;
using System;

namespace AutoService.Infrastructure.Services
{
    public class ExcelExportService : IExcelExportService
    {
        public byte[] GenerateMasterWorkloadReport(MasterWorkloadDto reportData)
        {
            throw new NotImplementedException();
        }

        public byte[] GenerateRepairsReport(ReportDataDto reportData)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("RepairsReport");

            // Заголовок отчета
            worksheet.Cells["A1"].Value = $"Отчет по ремонтам за {reportData.Month}.{reportData.Year}";
            worksheet.Cells["A1:E1"].Merge = true;
            worksheet.Cells["A1"].Style.Font.Bold = true;

            // Заголовки столбцов
            string[] headers = { "Дата", "Мастер", "Автомобиль", "Стоимость", "% загрузки" };
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[2, i + 1].Value = headers[i];
                worksheet.Cells[2, i + 1].Style.Font.Bold = true;
            }

            // Данные
            int row = 3;
            foreach (var repair in reportData.Repairs)
            {
                worksheet.Cells[row, 1].Value = repair.RepairStartDate.ToString("dd.MM.yyyy");
                worksheet.Cells[row, 2].Value = repair.MasterFullName;
                worksheet.Cells[row, 3].Value = $"{repair.CarMake} {repair.CarModel}";
                worksheet.Cells[row, 4].Value = repair.Cost;
                worksheet.Cells[row, 5].Value = repair.MasterWorkloadPercentage / 100;

                // Форматирование
                worksheet.Cells[row, 4].Style.Numberformat.Format = "#,##0.00 ₽";
                worksheet.Cells[row, 5].Style.Numberformat.Format = "0.00%";

                row++;
            }

            // Автоподбор ширины (альтернатива Dimension)
           // worksheet.Cells[worksheet.Dimension.Start.Row, worksheet.Dimension.Start.Column,
                        //  row, headers.Length].AutoFitColumns();

            return package.GetAsByteArray();
        }
    }
}