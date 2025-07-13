using AutoService.Core.Entities;

namespace AutoService.Core.Interfaces
{
    public interface 
        IRepairRepository
    {
        Task<IEnumerable<Repair>> GetInProgressRepairsForMonth(int month, int year);
        Task<decimal> GetMasterWorkloadPercentage(int masterId, int month, int year);
    }
}
