using Health.Claims.Service.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Health.Claims.Service.Infrastructure.Data.Context
{
    public class ClaimsDataServiceDBContext : DbContext
    {
        public ClaimsDataServiceDBContext(DbContextOptions<ClaimsDataServiceDBContext> options) : base(options) { }

        // DbSet properties for your entities
        public DbSet<ClaimRecordEntity> ClaimsRecords { get; set; } = default!;
        public DbSet<ClaimantEntity> Claimants { get; set; } = default!;
        public DbSet<PolicyEntity> Policies { get; set; } = default!;
        public DbSet<ClaimStatusEntity> ClaimStatuses { get; set; } = default!;
        public DbSet<DocumentEntity> Documents { get; set; } = default!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // This single line tells EF Core to scan the assembly for all
            // classes that implement IEntityTypeConfiguration and apply them.
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}