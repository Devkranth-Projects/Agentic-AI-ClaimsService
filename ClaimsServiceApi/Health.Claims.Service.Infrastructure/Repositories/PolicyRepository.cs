using Health.Claims.Service.Domain.Entities;
using Health.Claims.Service.Domain.Interfaces;
using Health.Claims.Service.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Health.Claims.Service.Infrastructure.Repositories
{
    public class PolicyRepository : IPolicyRepository
    {
        private readonly ClaimsDataServiceDBContext _context;

        public PolicyRepository(ClaimsDataServiceDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Get all policies, optionally including deleted records
        /// </summary>
        public async Task<IEnumerable<PolicyEntity>> GetAllPolicyDetails(bool includeDeleted = false)
        {
            var query = _context.Policies.AsQueryable();
            if (!includeDeleted)
                query = query.Where(p => p.IsDeleted == false);

            return await query. Include(m=>m.Claimant).ToListAsync();
        }

        /// <summary>
        /// Get policy by its ID
        /// </summary>
        public async Task<PolicyEntity?> GetPolicyDetailsByPolicyId(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Policy ID cannot be empty.", nameof(id));

            return await _context.Policies
                .Include(p => p.Claims)
                .Include(p => p.Claimant)
                .FirstOrDefaultAsync(p => p.EntityId == id && p.IsDeleted == false);
        }

        /// <summary>
        /// Add a new policy
        /// </summary>
        public async Task AddNewPolicy(PolicyEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.EntityId = Guid.NewGuid();
            entity.CreatedDt = DateTime.UtcNow;

            // TODO: Set CreatedBy from security context
            // entity.CreatedBy = currentUserId;

            await _context.Policies.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Update existing policy
        /// </summary>
        public async Task UpdateExistingPolicy(PolicyEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.UpdatedDt = DateTime.UtcNow;
            _context.Policies.Update(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Soft delete policy by ID
        /// </summary>
        public async Task DeleteExistingPolicy(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Policy ID cannot be empty.", nameof(id));

            var entity = await _context.Policies.FindAsync(id);
            if (entity == null)
                throw new InvalidOperationException($"Policy {id} not found");

            entity.IsDeleted =true;
            entity.UpdatedDt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task<bool> PolicyExistsAsync(Guid claimantId, string policyNumber)
        {
            return await _context.Policies
                .AnyAsync(p => p.ClaimantId == claimantId && p.PolicyNumber == policyNumber);
        }

        public async Task<PolicyEntity?> GetPolicyDetailsByPolicyId(Guid id, bool includeClaimant = false)
        {
            IQueryable<PolicyEntity> query = _context.Policies.AsQueryable();

            if (includeClaimant)
                query = query.Include(p => p.Claimant);

            return await query.FirstOrDefaultAsync(p => p.EntityId == id);
        }
    }
}
