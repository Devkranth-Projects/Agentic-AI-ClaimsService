using Health.Claims.Service.Domain.Entities;
using Health.Claims.Service.Domain.Interfaces;
using Health.Claims.Service.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Health.Claims.Service.Infrastructure.Repositories
{
    public class ClaimStatusRepository : IClaimStatusRepository
    {
        private readonly ClaimsDataServiceDBContext _context;

        public ClaimStatusRepository(ClaimsDataServiceDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<ClaimStatusEntity?> GetClaimStatusDetailsByClaimIdAsync(Guid claimId)
        {
            if (claimId == Guid.Empty)
                throw new ArgumentException("Claim ID cannot be empty.", nameof(claimId));

            // Find claim status by claimId
            return await _context.ClaimsRecords
                .Include(c => c.Status)
                .Where(c => c.EntityId == claimId && c.IsDeleted == false)
                .Select(c => c.Status)
                .FirstOrDefaultAsync();
        }

        public async Task AddNewClaimStatusAsync(ClaimStatusEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.EntityId = Guid.NewGuid();

            await _context.ClaimStatuses.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
    }
}
