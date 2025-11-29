using Health.Claims.Service.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Health.Claims.Service.Infrastructure.Data.Configurations
{
    public class DocumentConfiguration : IEntityTypeConfiguration<DocumentEntity>
    {
        public void Configure(EntityTypeBuilder<DocumentEntity> builder)
        {
            // Table name + schema
            builder.ToTable("Documents", "ClaimsService");

            // Primary key
            builder.HasKey(d => d.EntityId);

            // Properties
            builder.Property(d => d.FileName)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(d => d.FilePath)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(d => d.FileType)
                   .IsRequired()
                   .HasMaxLength(50);

            // Relationships
            builder.HasOne(d => d.Claim)               // navigation in DocumentEntity
                   .WithMany(c => c.Documents)         // navigation in ClaimRecordEntity
                   .HasForeignKey(d => d.ClaimId)      // FK in DocumentEntity
                   .OnDelete(DeleteBehavior.Cascade);  // delete documents when claim is deleted
        }
    }
}

