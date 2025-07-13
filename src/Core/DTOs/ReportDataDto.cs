namespace AutoService.Core.DTOs
{
    public class ReportDataDto
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public IEnumerable<RepairItemDto> Repairs { get; set; }
        public decimal GrandTotalCost { get; set; }
    }

    public class RepairItemDto
    {
        // Обязательные свойства
        public DateTime RepairStartDate { get; set; }
        public string CarMake { get; set; }
        public string CarModel { get; set; } // Добавлено это свойство
        public string LicensePlate { get; set; }
        public string MasterFullName { get; set; }
        public string ProblemDescription { get; set; }
        public decimal Cost { get; set; }
        public decimal MasterWorkloadPercentage { get; set; }
    }

    public class MasterWorkloadDto
    {
        public int MasterId { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public int ExperienceYears { get; set; }

        // Показатели загруженности
        public int RepairsCount { get; set; }
        public decimal TotalWorkCost { get; set; }
        public decimal WorkloadPercentage { get; set; } // 0-100%

        // Детализация по авто
        public IEnumerable<CarWorkloadDto> CarsWorkload { get; set; }
    }

    public class CarWorkloadDto
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public int RepairsCount { get; set; }
        public decimal TotalCost { get; set; }
    }
}