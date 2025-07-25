using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Infrastructure.Persistence.Configurations;

public class CoachConfiguration : IEntityTypeConfiguration<Coach>
{
    public void Configure(EntityTypeBuilder<Coach> builder)
    {
        builder.ToTable("Coaches");

        builder.HasKey(e => e.Id)
            .HasName("PK_Coaches");

        builder.Property(c => c.Name)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.Email)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.Slug)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(c => c.Slug)
            .IsUnique()
            .HasDatabaseName("IX_Coaches_Slug_Unique");

        builder.Property(c => c.IsActive)
            .HasColumnType("BIT")
            .IsRequired();

        builder.Property(c => c.SubscriptionStatus)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.SubscriptionPlan)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.SubscriptionEndAt)
            .HasColumnType("DATETIME2")
            .IsRequired(false);

        builder.Property(c => c.StripeCustomerId)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(256)
            .IsRequired(false);

        builder.Property(c => c.StripeSubscriptionId)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(256)
            .IsRequired(false);

        builder.HasOne(c => c.Address)
            .WithOne(a => a.Coach)
            .HasForeignKey<Address>(a => a.CoachId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(c => c.User)
            .WithOne()
            .HasForeignKey<Coach>(c => c.Id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}