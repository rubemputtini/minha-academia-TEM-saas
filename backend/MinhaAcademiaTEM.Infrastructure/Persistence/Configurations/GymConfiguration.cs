using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Infrastructure.Persistence.Configurations;

public class GymConfiguration : IEntityTypeConfiguration<Gym>
{
    public void Configure(EntityTypeBuilder<Gym> builder)
    {
        builder.ToTable("Gyms");

        builder.HasKey(g => g.Id)
            .HasName("PK_Gyms");

        builder.Property(g => g.Name)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(g => g.Location)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(100)
            .IsRequired();

        builder.HasOne(g => g.User)
            .WithOne(u => u.Gym)
            .HasForeignKey<Gym>(g => g.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}