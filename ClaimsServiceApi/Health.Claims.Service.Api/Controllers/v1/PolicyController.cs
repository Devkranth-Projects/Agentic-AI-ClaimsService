using AutoMapper;
using Health.Claims.Service.Application.DataTransferObjects;
using Health.Claims.Service.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Health.Claims.Service.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PolicyController : ControllerBase
    {
        private readonly IPolicyService _policyService;
        private readonly IMapper _mapper;
        private readonly ILogger<PolicyController> _logger;

        public PolicyController(
            IPolicyService policyService,
            IMapper mapper,
            ILogger<PolicyController> logger)
        {
            _policyService = policyService ?? throw new ArgumentNullException(nameof(policyService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // 🔹 1. GET ALL POLICIES
        [HttpGet]
        public async Task<IActionResult> GetAllPolicies([FromQuery] bool includeDeleted = false)
        {
            try
            {
                var policies = await _policyService.GetAllPoliciesAsync(includeDeleted);
                var response = _mapper.Map<IEnumerable<PolicyDto>>(policies);

                return Ok(new ApiResponse<IEnumerable<PolicyDto>>(response, true, "Policies fetched successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching policies");
                return Ok(new ApiResponse<IEnumerable<PolicyDto>>(null, false, "Unable to fetch policies"));
            }
        }

        // 🔹 2. GET BASIC POLICY DETAILS BY ID
        [HttpGet("basic/{id:guid}")]
        public async Task<IActionResult> GetPolicyById(Guid id)
        {
            if (id == Guid.Empty)
                return Ok(new ApiResponse<PolicyDto>(null, false, "Policy ID cannot be empty"));

            try
            {
                var policy = await _policyService.GetPolicyByIdAsync(id);
                if (policy == null)
                    return Ok(new ApiResponse<PolicyDto>(null, false, $"Policy with ID {id} not found"));

                var response = _mapper.Map<PolicyDto>(policy);
                return Ok(new ApiResponse<PolicyDto>(response, true, "Policy fetched successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching policy by ID {Id}", id);
                return Ok(new ApiResponse<PolicyDto>(null, false, "Error fetching policy"));
            }
        }

        // 🔹 3. GET POLICY DETAILS WITH CLAIMANT INFO
        [HttpGet("details/{policyId:guid}")]
        public async Task<IActionResult> GetPolicyDetailsWithClaimantById(Guid policyId)
        {
            if (policyId == Guid.Empty)
                return Ok(new ApiResponse<PolicyDto>(null, false, "Policy ID cannot be empty"));

            try
            {
                var policy = await _policyService.GetPolicyByIdAsync(policyId, includeClaimant: true);
                if (policy == null)
                    return Ok(new ApiResponse<PolicyDto>(null, false, $"Policy with ID {policyId} not found"));

                var response = _mapper.Map<PolicyDto>(policy);
                return Ok(new ApiResponse<PolicyDto>(response, true, "Policy with claimant details fetched successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching policy with claimant by ID {PolicyId}", policyId);
                return Ok(new ApiResponse<PolicyDto>(null, false, "Error fetching policy details"));
            }
        }

        // 🔹 4. ADD NEW POLICY
        [HttpPost]
        public async Task<IActionResult> AddPolicy([FromBody] PolicyDto dto)
        {
            if (!ModelState.IsValid)
                return Ok(new ApiResponse<PolicyDto>(null, false, "Validation failed"));

            try
            {
                if (await _policyService.PolicyExistsAsync(dto.ClaimantId, dto.PolicyNumber))
                    return Ok(new ApiResponse<PolicyDto>(null, false, $"Policy number '{dto.PolicyNumber}' already exists for this claimant."));

                if (dto.PolicyId == Guid.Empty)
                    dto.PolicyId = Guid.NewGuid();

                var entity = _mapper.Map<Domain.Entities.PolicyEntity>(dto);
                await _policyService.AddPolicyAsync(entity);

                var response = _mapper.Map<PolicyDto>(entity);
                return Ok(new ApiResponse<PolicyDto>(response, true, "Policy added successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding policy");
                return Ok(new ApiResponse<PolicyDto>(null, false, "Failed to add policy"));
            }
        }

        // 🔹 5. UPDATE POLICY
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdatePolicy(Guid id, [FromBody] PolicyDto dto)
        {
            if (id == Guid.Empty || dto == null)
                return Ok(new ApiResponse<object>(null, false, "Invalid policy ID or data"));

            if (id != dto.PolicyId)
                return Ok(new ApiResponse<object>(null, false, "Policy ID mismatch"));

            try
            {
                var entity = _mapper.Map<Domain.Entities.PolicyEntity>(dto);
                await _policyService.UpdatePolicyAsync(entity);

                return Ok(new ApiResponse<object>(null, true, "Policy updated successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating policy");
                return Ok(new ApiResponse<object>(null, false, "Failed to update policy"));
            }
        }

        // 🔹 6. DELETE POLICY
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeletePolicy(Guid id)
        {
            if (id == Guid.Empty)
                return Ok(new ApiResponse<object>(null, false, "Policy ID cannot be empty"));

            try
            {
                await _policyService.DeletePolicyAsync(id);
                return Ok(new ApiResponse<object>(null, true, "Policy deleted successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting policy");
                return Ok(new ApiResponse<object>(null, false, "Failed to delete policy"));
            }
        }
    }
}
