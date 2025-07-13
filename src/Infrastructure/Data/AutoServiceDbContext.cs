using AutoService.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoService.Infrastructure.Data
{
    public class AutoServiceDbContext : DbContext
    {
        public AutoServiceDbContext(DbContextOptions<AutoServiceDbContext> options) : base(options) { }

        public DbSet<Master> Masters { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarOwner> CarOwners { get; set; }
        public DbSet<Repair> Repairs { get; set; }

        // Представление (только для чтения)
        public DbSet<MayInProgressRepairsView> MayInProgressRepairsViews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AutoServiceDbContext).Assembly);

            // Настройка представления
            modelBuilder.Entity<MayInProgressRepairsView>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("MayInProgressRepairsView");
            });

            var methodInfo = typeof(AutoServiceDbContext)
        .GetMethod(nameof(GetMasterWorkloadPercentage));

            modelBuilder.HasDbFunction(methodInfo)
                .HasName("GetMasterWorkloadPercentage")
                .HasSchema("dbo");
        }

        // Метод для вызова функции БД
        public decimal GetMasterWorkloadPercentage(int masterId, int month, int year)
            => throw new NotSupportedException("Вызывается только в LINQ-to-Entities");
    }
}