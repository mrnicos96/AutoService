namespace AutoService.Core.Entities
{
    /// <summary>
    /// Модель для представления MayInProgressRepairsView
    /// Не является сущностью БД, только для чтения
    /// </summary>
    public class MayInProgressRepairsView
    {
        public DateTime RepairStartDate { get; set; }
        public string CarMake { get; set; }
        public string CarModel { get; set; }
        public string LicensePlate { get; set; }
        public string OwnerFullName { get; set; }
        public string Problem { get; set; }
        public decimal Cost { get; set; }
        public string MasterFullName { get; set; }
        public string MasterPhone { get; set; }
        public int MasterExperience { get; set; }
        public decimal MasterTotalCost { get; set; }
        public decimal AllMastersTotalCost { get; set; }
        public decimal MasterWorkloadPercentage { get; set; }
    }
}