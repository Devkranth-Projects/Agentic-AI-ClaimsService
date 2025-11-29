using Health.Claims.Service.Domain.Entities;
using Health.Claims.Service.Domain.Interfaces;
using Health.Claims.Service.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Health.Claims.Service.Infrastructure.Repositories
{
    public class ClaimRepository : IClaimRepository
    {
        private readonly ClaimsDataServiceDBContext _context;

        public ClaimRepository(ClaimsDataServiceDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<ClaimRecordEntity?> GetDetailsByClaimIdAsync(Guid claimId)
        {
            if (claimId == Guid.Empty)
                throw new ArgumentException("Claim ID cannot be empty.", nameof(claimId));

            return await _context.ClaimsRecords
                .Include(c => c.Claimant)  // eager-load Claimant
                .Include(c => c.Policy)    // eager-load Policy
                .Include(c => c.Status)    // eager-load Status
                .Include(c => c.Documents) // eager-load Documents
                .FirstOrDefaultAsync(c => c.EntityId == claimId && c.IsDeleted == false);
        }

        public async Task SubmitNewClaimAsync(ClaimRecordEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.EntityId = Guid.NewGuid();
            entity.CreatedDt = DateTime.UtcNow;

            // TODO: Set CreatedBy from security context
            // entity.CreatedBy = currentUserId;

            await _context.ClaimsRecords.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
    }
}
