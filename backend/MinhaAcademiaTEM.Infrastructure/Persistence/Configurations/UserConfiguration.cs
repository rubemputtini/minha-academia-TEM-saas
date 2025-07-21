using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("AspNetUsers");

        builder.Property(u => u.Name)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(80)
            .IsRequired();

        builder.Property(u => u.Role)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(20)
            .IsRequired();

        builder.HasOne(u => u.Coach)
            .WithMany()
            .HasForeignKey(u => u.CoachId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.EquipmentSelections)
            .WithOne(es => es.User)
            .HasForeignKey(es => es.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}