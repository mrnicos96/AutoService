using AutoService.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoService.Infrastructure.Data.Configurations
{
    public class CarOwnerConfiguration : IEntityTypeConfiguration<CarOwner>
    {
        public void Configure(EntityTypeBuilder<CarOwner> builder)
        {
            builder.ToTable("CarOwners");

            builder.HasKey(o => o.OwnerId);

            // Настройка свойств
            builder.Property(o => o.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(o => o.PassportData)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(o => o.Phone)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);

            // Уникальные индексы
            builder.HasIndex(o => o.Phone)
                .IsUnique();

            builder.HasIndex(o => o.PassportData)
                .IsUnique();

            builder.HasIndex(o => new { o.LastName, o.FirstName, o.MiddleName })
                .IsUnique()
                .HasDatabaseName("UQ_OwnerFullName");

            // Навигационные свойства
            builder.HasMany(o => o.Cars)
                .WithOne(c => c.Owner)
                .HasForeignKey(c => c.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}