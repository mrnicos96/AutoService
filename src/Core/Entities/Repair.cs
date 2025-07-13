using System.Diagnostics.Metrics;

namespace AutoService.Core.Entities
{
    public class Repair
    {
        public int RepairId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; } // "InProgress", "Completed"
        public string ProblemDescription { get; set; }
        public decimal WorkCost { get; set; }

        // Навигационные свойства
        public int CarId { get; set; }
        public Car Car { get; set; }

        public int MasterId { get; set; }
        public Master Master { get; set; }
    }
}