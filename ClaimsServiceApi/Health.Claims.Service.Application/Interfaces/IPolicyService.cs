using Health.Claims.Service.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health.Claims.Service.Application.Interfaces
{
    /// <summary>
    /// Defines application-level operations for Policy management.
    /// </summary>
    public interface IPolicyService
    {
        /// <summary>
        /// Retrieves all policies, optionally including soft-deleted records.
        /// </summary>
        Task<IEnumerable<PolicyEntity>> GetAllPoliciesAsync(bool includeDeleted = false);

        /// <summary>
        /// Retrieves a single policy by its unique identifier.
        /// </summary>
        Task<PolicyEntity?> GetPolicyByIdAsync(Guid policyId);

        /// <summary>
        /// Creates a new policy.
        /// </summary>
        Task AddPolicyAsync(PolicyEntity policy);

        /// <summary>
        /// Updates an existing policy.
        /// </summary>
        Task UpdatePolicyAsync(PolicyEntity policy);

        /// <summary>
        /// Soft-deletes a policy by its ID.
        /// </summary>
        Task DeletePolicyAsync(Guid policyId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="claimantId"></param>
        /// <param name="policyNumber"></param>
        /// <returns></returns>
        Task<bool> PolicyExistsAsync(Guid claimantId, string policyNumber);

        Task<PolicyEntity?> GetPolicyByIdAsync(Guid policyId, bool includeClaimant= true);
    }
}
