using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Infrastructure.Persistence.Configurations;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("Addresses");

        builder.HasKey(a => a.Id)
            .HasName("PK_Addresses");

        builder.Property(a => a.Street)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(a => a.Number)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(a => a.Complement)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(100);

        builder.Property(a => a.Neighborhood)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(60)
            .IsRequired();

        builder.Property(a => a.City)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(60)
            .IsRequired();

        builder.Property(a => a.State)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(a => a.Country)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(a => a.PostalCode)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(a => a.Latitude)
            .HasColumnType("FLOAT");

        builder.Property(a => a.Longitude)
            .HasColumnType("FLOAT");

        builder.HasOne(a => a.Coach)
            .WithOne(c => c.Address)
            .HasForeignKey<Address>(a => a.CoachId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}