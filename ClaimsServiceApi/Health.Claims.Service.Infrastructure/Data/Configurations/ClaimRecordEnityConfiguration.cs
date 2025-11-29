using Health.Claims.Service.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Health.Claims.Service.Infrastructure.Data.Configurations
{
    public class ClaimRecordEntityConfiguration : IEntityTypeConfiguration<ClaimRecordEntity>
    {
        public void Configure(EntityTypeBuilder<ClaimRecordEntity> builder)
        {
            // Table and schema
            builder.ToTable("Claims", "ClaimsService");

            // Primary Key
            builder.HasKey(c => c.EntityId);

            // Properties
            builder.Property(c => c.Description)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(c => c.Amount)
                   .IsRequired();

            builder.Property(c => c.DateOfIncident)
                   .IsRequired();

            builder.Property(c => c.IncidentLocation)
                   .HasMaxLength(250);

            // Relationships
            builder.HasOne(c => c.Claimant)
                   .WithMany(ca => ca.Claims)
                   .HasForeignKey(c => c.ClaimantId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Policy)
                   .WithMany(p => p.Claims)
                   .HasForeignKey(c => c.PolicyId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Status)
                   .WithMany(s => s.Claims)
                   .HasForeignKey(c => c.StatusId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Documents)
                   .WithOne(d => d.Claim)
                   .HasForeignKey(d => d.ClaimId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
