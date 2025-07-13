using System.ComponentModel.DataAnnotations;

namespace AutoService.Core.Enums
{
    public enum RepairStatus
    {
        [Display(Name = "В работе")]
        InProgress,

        [Display(Name = "Завершен")]
        Completed,

        [Display(Name = "Отменен")]
        Cancelled
    }
}