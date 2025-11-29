using Health.Claims.Service.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Health.Claims.Service.Infrastructure.Data.Configurations
{
    public class ClaimantConfiguration : IEntityTypeConfiguration<ClaimantEntity>
    {
        public void Configure(EntityTypeBuilder<ClaimantEntity> builder)
        {
            // Table name and schema
            builder.ToTable("Claimants", "ClaimsService");

            // Primary Key
            builder.HasKey(c => c.EntityId);

            // Personal Info
            builder.Property(c => c.FirstName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(c => c.MiddleName)
                   .HasMaxLength(100);

            builder.Property(c => c.LastName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(c => c.Dob)
                   .IsRequired();

            builder.Property(c => c.MaritalStatus)
                   .HasMaxLength(50);

            builder.Property(c => c.Nationality)
                   .HasMaxLength(100);

            // Contact Info
            builder.Property(c => c.Email)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(c => c.ConfirmEmail)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(c => c.Phone)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(c => c.AltPhone)
                   .HasMaxLength(50);

            // Address
            builder.Property(c => c.AddressLine1)
                   .HasMaxLength(200);

            builder.Property(c => c.AddressLine2)
                   .HasMaxLength(200);

            builder.Property(c => c.City)
                   .HasMaxLength(100);

            builder.Property(c => c.State)
                   .HasMaxLength(100);

            builder.Property(c => c.Zip)
                   .HasMaxLength(20);

            builder.Property(c => c.Country)
                   .HasMaxLength(100);

            // Identification
            builder.Property(c => c.Passport)
                   .HasMaxLength(50);

            builder.Property(c => c.DriverLicense)
                   .HasMaxLength(50);

            builder.Property(c => c.TaxId)
                   .HasMaxLength(50);

            // Credit Card Info
            builder.Property(c => c.CardNumber)
                   .HasMaxLength(20);

            builder.Property(c => c.CardExpiry)
                   .HasMaxLength(10);

            builder.Property(c => c.CardCVV)
                   .HasMaxLength(5);

            builder.Property(c => c.CardHolder)
                   .HasMaxLength(150);

            // Additional Info
            builder.Property(c => c.Notes)
                   .HasMaxLength(1000);          

            // Relationships: Claimant has many Claims
            builder.HasMany(c => c.Claims)
                   .WithOne(c => c.Claimant)
                   .HasForeignKey(c => c.ClaimantId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Relationships: Claimant has many Policies
            builder.HasMany(c => c.Policies)
                   .WithOne(p => p.Claimant)
                   .HasForeignKey(p => p.ClaimantId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
