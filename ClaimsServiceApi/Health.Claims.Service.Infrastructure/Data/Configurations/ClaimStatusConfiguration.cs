using Health.Claims.Service.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Health.Claims.Service.Infrastructure.Data.Configurations
{
    public class ClaimStatusConfiguration : IEntityTypeConfiguration<ClaimStatusEntity>
    {
        public void Configure(EntityTypeBuilder<ClaimStatusEntity> builder)
        {
            // Table and schema
            builder.ToTable("ClaimStatuses", "ClaimsService");

            // Primary Key
            builder.HasKey(c => c.EntityId);

            // Properties
            builder.Property(c => c.StatusName)
                   .IsRequired()
                   .HasMaxLength(100);

            // Relationships
            builder.HasMany(c => c.Claims)        // navigation property on ClaimStatusEntity
                   .WithOne(cl => cl.Status)      // navigation property on ClaimRecordEntity
                   .HasForeignKey(cl => cl.StatusId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
