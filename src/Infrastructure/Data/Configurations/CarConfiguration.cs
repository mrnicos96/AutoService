using AutoService.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoService.Infrastructure.Data.Configurations
{
    public class CarConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.HasKey(c => c.CarId);

            builder.Property(c => c.Make)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.LicensePlate)
                .IsRequired()
                .HasMaxLength(15);

            // Уникальный индекс для комбинации полей
            builder.HasIndex(c => new { c.Make, c.Model, c.LicensePlate })
                .IsUnique();

            // Связь с владельцем
            builder.HasOne(c => c.Owner)
                .WithMany(o => o.Cars)
                .HasForeignKey(c => c.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}