using AutoService.Core.Entities;
using AutoService.Core.Interfaces;
using AutoService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AutoService.Infrastructure.Repositories
{
    public class RepairRepository : IRepairRepository
    {
        private readonly AutoServiceDbContext _context;

        public RepairRepository(AutoServiceDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Repair>> GetInProgressRepairsForMonth(int month, int year)
        {
            return await _context.Repairs
                .AsNoTracking()
                .Include(r => r.Car)
                    .ThenInclude(c => c.Owner)
                .Include(r => r.Master)
                .Where(r => r.Status == "InProgress" &&
                           r.StartDate.Month == month &&
                           r.StartDate.Year == year)
                .OrderBy(r => r.StartDate)
                .ThenBy(r => r.Master.LastName)
                .ThenBy(r => r.Car.Make)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<decimal> GetMasterWorkloadPercentage(int masterId, int month, int year)
        {
            return await _context.Masters
                .Where(m => m.MasterId == masterId)
                .Select(m => _context.GetMasterWorkloadPercentage(m.MasterId, month, year))
                .FirstOrDefaultAsync();
        }
    }
}