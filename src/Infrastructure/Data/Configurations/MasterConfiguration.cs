using AutoService.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoService.Infrastructure.Data.Configurations
{
    public class MasterConfiguration : IEntityTypeConfiguration<Master>
    {
        public void Configure(EntityTypeBuilder<Master> builder)
        {
            builder.ToTable("Masters"); // Явное указание имени таблицы

            builder.HasKey(m => m.MasterId); // Первичный ключ

            // Настройка свойств
            builder.Property(m => m.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(m => m.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(m => m.MiddleName)
                .HasMaxLength(50);

            builder.Property(m => m.Phone)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(m => m.ExperienceYears)
                .HasDefaultValue(0);

            // Уникальные индексы
            builder.HasIndex(m => m.Phone)
                .IsUnique();

            builder.HasIndex(m => new { m.LastName, m.FirstName, m.MiddleName })
                .IsUnique()
                .HasDatabaseName("UQ_MasterFullName");

            // Навигационные свойства
            builder.HasMany(m => m.Repairs)
                .WithOne(r => r.Master)
                .HasForeignKey(r => r.MasterId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}