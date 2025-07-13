using System.ComponentModel.DataAnnotations;

namespace AutoService.Core.Entities
{
    public class CarOwner
    {
        public int OwnerId { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string? MiddleName { get; set; }

        [Required]
        [StringLength(20)]
        public string Phone { get; set; }

        [Required]
        [StringLength(200)]
        public string Address { get; set; }

        [Required]
        [StringLength(100)]
        public string PassportData { get; set; }

        // Навигационное свойство для автомобилей
        public ICollection<Car> Cars { get; set; } = new List<Car>();

        // Вычисляемое свойство
        public string FullName => $"{LastName} {FirstName} {MiddleName}".Trim();
    }
}