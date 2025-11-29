using Health.Claims.Service.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Health.Claims.Service.Domain.Interfaces
{
    public interface IPolicyRepository
    {
        Task<IEnumerable<PolicyEntity>> GetAllPolicyDetails(bool includeDeleted = false);
        Task<PolicyEntity?> GetPolicyDetailsByPolicyId(Guid id);
        Task AddNewPolicy(PolicyEntity entity);
        Task UpdateExistingPolicy(PolicyEntity entity);
        Task DeleteExistingPolicy(Guid id);

        // New method to check if a policy exists for a claimant
        Task<bool> PolicyExistsAsync(Guid claimantId, string policyNumber);

        Task<PolicyEntity?> GetPolicyDetailsByPolicyId(Guid id, bool includeClaimant = false);

    }
}
