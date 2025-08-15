using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Infrastructure.Persistence.Configurations;

public class EquipmentNoteConfiguration : IEntityTypeConfiguration<EquipmentNote>
{
    public void Configure(EntityTypeBuilder<EquipmentNote> builder)
    {
        builder.ToTable("EquipmentNotes");

        builder.HasKey(en => en.Id)
            .HasName("PK_EquipmentNotes");

        builder.Property(en => en.Message)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(en => en.UserId)
            .IsRequired();

        builder.Property(en => en.CoachId)
            .IsRequired();

        builder.HasOne(en => en.User)
            .WithMany()
            .HasForeignKey(en => en.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasOne<Coach>()
            .WithMany()
            .HasForeignKey(en => en.CoachId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(n => new { n.CoachId, n.UserId })
            .IsUnique()
            .HasDatabaseName("UX_EquipmentNotes_Coach_User");
    }
}