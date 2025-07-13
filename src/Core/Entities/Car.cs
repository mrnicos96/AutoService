using System.ComponentModel.DataAnnotations;

namespace AutoService.Core.Entities
{
    public class Car
    {
        public int CarId { get; set; }

        [Required]
        [StringLength(50)]
        public string Make { get; set; } // Марка (Toyota)

        [Required]
        [StringLength(50)]
        public string Model { get; set; } // Модель (Camry)

        [Required]
        [StringLength(15)]
        public string LicensePlate { get; set; } // Гос. номер

        [Range(0.5, 10.0)]
        public decimal EngineVolume { get; set; } // Объем двигателя

        [Range(0, 1_000_000)]
        public int Mileage { get; set; } // Пробег

        // Внешний ключ и навигационное свойство
        public int OwnerId { get; set; }
        public CarOwner Owner { get; set; }

        // Навигационное свойство для ремонтов
        public ICollection<Repair> Repairs { get; set; } = new List<Repair>();
    }
}