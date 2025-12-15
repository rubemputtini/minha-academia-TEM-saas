using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Infrastructure.Persistence.Configurations;

public class ReferralAccountConfiguration : IEntityTypeConfiguration<ReferralAccount>
{
    public void Configure(EntityTypeBuilder<ReferralAccount> builder)
    {
        builder.ToTable("ReferralAccounts");

        builder.HasKey(ra => ra.Id)
            .HasName("PK_ReferralAccounts");

        builder.Property(ra => ra.CreditsAvailable)
            .HasColumnType("INT")
            .IsRequired();
        
        builder.Property(ra => ra.TotalCreditsEarned)
            .HasColumnType("INT")
            .IsRequired();

        builder.Property(ra => ra.LastAppliedPeriod)
            .HasColumnType("INT")
            .IsRequired(false);

        builder.Property(ra => ra.UpdatedAt)
            .HasColumnType("DATETIME2")
            .IsRequired(false);

        builder.HasIndex(ra => ra.CoachId).IsUnique();

        builder.HasOne(ra => ra.Coach)
            .WithOne()
            .HasForeignKey<ReferralAccount>(ra => ra.CoachId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}