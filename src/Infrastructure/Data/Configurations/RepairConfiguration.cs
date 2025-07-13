using AutoService.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoService.Infrastructure.Data.Configurations
{
    public class RepairConfiguration : IEntityTypeConfiguration<Repair>
    {
        public void Configure(EntityTypeBuilder<Repair> builder)
        {
            builder.ToTable("Repairs");

            builder.HasKey(r => r.RepairId);

            // Настройка свойств
            builder.Property(r => r.StartDate)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(r => r.EndDate)
                .IsRequired(false);

            builder.Property(r => r.ProblemDescription)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(r => r.WorkCost)
                .IsRequired()
                .HasColumnType("decimal(10,2)")
                .HasDefaultValue(0);

            builder.Property(r => r.Status)
                .IsRequired()
                .HasMaxLength(20)
                .HasConversion<string>(); // Для enum

            // Проверочные ограничения
            builder.HasCheckConstraint(
                "CHK_RepairDates",
                "EndDate IS NULL OR EndDate >= StartDate");

            builder.HasCheckConstraint(
                "CHK_WorkCost",
                "WorkCost >= 0");

            // Внешние ключи
            builder.HasOne(r => r.Car)
                .WithMany(c => c.Repairs)
                .HasForeignKey(r => r.CarId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Master)
                .WithMany(m => m.Repairs)
                .HasForeignKey(r => r.MasterId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}