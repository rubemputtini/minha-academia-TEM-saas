using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Infrastructure.Persistence.Configurations;

public class BaseEquipmentConfiguration : IEntityTypeConfiguration<BaseEquipment>
{
    public void Configure(EntityTypeBuilder<BaseEquipment> builder)
    {
        builder.ToTable("BaseEquipments");
        
        builder.HasKey(e => e.Id)
            .HasName("PK_BaseEquipments");
        
        builder.Property(e => e.Name)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.PhotoUrl)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(500)
            .IsRequired();
        
        builder.Property(e => e.VideoUrl)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(e => e.MuscleGroup)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(50)
            .IsRequired();
    }
}