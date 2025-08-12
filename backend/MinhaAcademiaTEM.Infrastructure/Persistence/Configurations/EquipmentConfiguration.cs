using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Infrastructure.Persistence.Configurations;

public class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
{
    public void Configure(EntityTypeBuilder<Equipment> builder)
    {
        builder.ToTable("Equipments");

        builder.HasKey(es => es.Id)
            .HasName("PK_Equipments");

        builder.HasAlternateKey(e => new { e.Id, e.CoachId })
            .HasName("AK_Equipments_Id_CoachId");

        builder.Property(e => e.Name)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.VideoUrl)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(e => e.MuscleGroup)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.IsActive)
            .HasColumnType("BIT")
            .IsRequired();

        builder.HasOne(e => e.BaseEquipment)
            .WithMany(be => be.Customizations)
            .HasForeignKey(e => e.BaseEquipmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}