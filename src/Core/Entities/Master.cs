using System.ComponentModel.DataAnnotations;

namespace AutoService.Core.Entities
{
    public class Master
    {
        public int MasterId { get; set; }

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

        [Range(0, 50)]
        public int ExperienceYears { get; set; }

        // Навигационное свойство для ремонтов
        public ICollection<Repair> Repairs { get; set; } = new List<Repair>();

        // Вычисляемое свойство (не маппится в БД)
        public string FullName => $"{LastName} {FirstName} {MiddleName}".Trim();
    }
}
