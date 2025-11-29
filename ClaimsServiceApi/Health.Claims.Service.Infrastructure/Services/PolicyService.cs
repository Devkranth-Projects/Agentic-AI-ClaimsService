using AutoMapper;
using Health.Claims.Service.Application.Interfaces;
using Health.Claims.Service.Domain.Entities;
using Health.Claims.Service.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Health.Claims.Service.Infrastructure.Services
{
    public class PolicyService : IPolicyService
    {
        private readonly IPolicyRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<PolicyService> _logger;

        public PolicyService(
            IPolicyRepository repository,
            IMapper mapper,
            ILogger<PolicyService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get all policies.
        /// </summary>
        public async Task<IEnumerable<PolicyEntity>> GetAllPoliciesAsync(bool includeDeleted = false)
        {
            _logger.LogInformation("Fetching all policies. IncludeDeleted={IncludeDeleted}", includeDeleted);
            var policies = await _repository.GetAllPolicyDetails(includeDeleted);
            return policies;
        }

        /// <summary>
        /// Get policy by ID.
        /// </summary>
        public async Task<PolicyEntity?> GetPolicyByIdAsync(Guid policyId)
        {
            if (policyId == Guid.Empty)
                throw new ArgumentException("Policy ID cannot be empty.", nameof(policyId));

            _logger.LogInformation("Fetching policy with ID {PolicyId}", policyId);
            var policy = await _repository.GetPolicyDetailsByPolicyId(policyId);
            return policy;
        }

        /// <summary>
        /// Add a new policy.
        /// </summary>
        public async Task AddPolicyAsync(PolicyEntity policy)
        {
            if (policy == null) throw new ArgumentNullException(nameof(policy));

            policy.EntityId = Guid.NewGuid();
            policy.CreatedDt = DateTime.UtcNow;

            _logger.LogInformation("Adding new policy {PolicyNumber}", policy.PolicyNumber);
            await _repository.AddNewPolicy(policy);
        }

        /// <summary>
        /// Update an existing policy.
        /// </summary>
        public async Task UpdatePolicyAsync(PolicyEntity policy)
        {
            if (policy == null) throw new ArgumentNullException(nameof(policy));
            if (policy.EntityId == Guid.Empty) throw new ArgumentException("Policy ID cannot be empty.", nameof(policy.EntityId));

            policy.UpdatedDt = DateTime.UtcNow;

            _logger.LogInformation("Updating policy {PolicyId}", policy.EntityId);
            await _repository.UpdateExistingPolicy(policy);
        }

        /// <summary>
        /// Soft delete a policy.
        /// </summary>
        public async Task DeletePolicyAsync(Guid policyId)
        {
            if (policyId == Guid.Empty) throw new ArgumentException("Policy ID cannot be empty.", nameof(policyId));

            _logger.LogInformation("Deleting policy {PolicyId}", policyId);
            await _repository.DeleteExistingPolicy(policyId);
        }

        public async Task<bool> PolicyExistsAsync(Guid claimantId, string policyNumber)
        {
            return await _repository.PolicyExistsAsync(claimantId, policyNumber);
        }

        public async Task<PolicyEntity?> GetPolicyByIdAsync(Guid policyId, bool includeClaimant = true)
        {
            return await _repository.GetPolicyDetailsByPolicyId(policyId, includeClaimant);
        }
    }
}
