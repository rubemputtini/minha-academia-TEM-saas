using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Infrastructure.Persistence.Configurations;

public class EquipmentSelectionConfiguration : IEntityTypeConfiguration<EquipmentSelection>
{
    public void Configure(EntityTypeBuilder<EquipmentSelection> builder)
    {
        builder.ToTable("EquipmentSelections");

        builder.HasKey(es => es.Id)
            .HasName("PK_EquipmentSelections");

        builder.Property(es => es.IsAvailable)
            .HasColumnType("BIT")
            .IsRequired();

        builder.HasOne(es => es.User)
            .WithMany(u => u.EquipmentSelections)
            .HasForeignKey(es => es.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(es => es.Equipment)
            .WithMany()
            .HasForeignKey(es => es.EquipmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}