using Health.Claims.Service.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Health.Claims.Service.Infrastructure.Data.Configurations
{
    public class PolicyConfiguration : IEntityTypeConfiguration<PolicyEntity>
    {
        public void Configure(EntityTypeBuilder<PolicyEntity> builder)
        {
            // Table + schema mapping
            builder.ToTable("Policies", "ClaimsService");

            // Primary key
            builder.HasKey(p => p.EntityId);

            // Properties
            builder.Property(p => p.PolicyNumber)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(p => p.PolicyType)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(p => p.EffectiveDate)
                   .IsRequired();

            builder.Property(p => p.ExpirationDate)
                   .IsRequired();

            builder.Property(p => p.Description)
                   .HasMaxLength(500);

            // Relationships
            builder.HasOne(p => p.Claimant)                 // Each policy belongs to one claimant
                   .WithMany(c => c.Policies)               // A claimant can have many policies
                   .HasForeignKey(p => p.ClaimantId)       // FK in PolicyEntity
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.Claims)                  // A policy can have many claims
                   .WithOne(c => c.Policy)                  // Each claim belongs to one policy
                   .HasForeignKey(c => c.PolicyId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
