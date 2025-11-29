using AutoMapper;
using Health.Claims.Service.Application.DataTransferObjects;
using Health.Claims.Service.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Health.Claims.Service.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ClaimantController : ControllerBase
    {
        private readonly IClaimantService _claimantService;
        private readonly ILogger<ClaimantController> _logger;

        public ClaimantController(
            IClaimantService claimantService,
            ILogger<ClaimantController> logger)
        {
            _claimantService = claimantService ?? throw new ArgumentNullException(nameof(claimantService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get all claimants.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ClaimantDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllClaimants([FromQuery] bool includeDeleted = false)
        {
            try
            {
                var claimants = await _claimantService.GetAllClaimantsAsync(includeDeleted);
                return Ok(new ApiResponse<IEnumerable<ClaimantDto>>(claimants, true, "Claimants fetched successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching claimants");
                return Ok(new ApiResponse<IEnumerable<ClaimantDto>>(null, false, "Unable to fetch claimants"));
            }
        }

        /// <summary>
        /// Get claimant by ID.
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<ClaimantDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetClaimantById(Guid id)
        {
            if (id == Guid.Empty)
                return Ok(new ApiResponse<ClaimantDto>(null, false, "Claimant ID cannot be empty"));

            try
            {
                var claimant = await _claimantService.GetClaimantByIdAsync(id);
                if (claimant == null)
                    return Ok(new ApiResponse<ClaimantDto>(null, false, $"Claimant with ID {id} not found"));

                return Ok(new ApiResponse<ClaimantDto>(claimant, true, "Claimant fetched successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching claimant by ID {Id}", id);
                return Ok(new ApiResponse<ClaimantDto>(null, false, "Error fetching claimant"));
            }
        }

        /// <summary>
        /// Create a new claimant. 
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ClaimantDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddClaimant([FromBody] ClaimantDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<ClaimantDto>(null, false, "Validation failed"));

            // Convert all DateTime fields to UTC
            dto.Dob = dto.Dob.ToUniversalTime();

            await _claimantService.AddClaimantAsync(dto);

            return Ok(new ApiResponse<ClaimantDto>(dto, true, "Claimant added successfully"));
        }

        /// <summary>
        /// Update existing claimant.
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateClaimant(Guid id, [FromBody] ClaimantDto dto)
        {
            if (id == Guid.Empty || dto == null)
                return Ok(new ApiResponse<object>(null, false, "Invalid claimant ID or data"));

            if (id != dto.ClaimantId)
                return Ok(new ApiResponse<object>(null, false, "Claimant ID in route and body do not match"));

            try
            {
                await _claimantService.UpdateClaimantAsync(dto);
                return Ok(new ApiResponse<object>(null, true, "Claimant updated successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating claimant");
                return Ok(new ApiResponse<object>(null, false, "Failed to update claimant"));
            }
        }

        /// <summary>
        /// Soft delete claimant by ID.
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteClaimant(Guid id)
        {
            if (id == Guid.Empty)
                return Ok(new ApiResponse<object>(null, false, "Claimant ID cannot be empty"));

            try
            {
                await _claimantService.DeleteClaimantAsync(id);
                return Ok(new ApiResponse<object>(null, true, "Claimant deleted successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting claimant");
                return Ok(new ApiResponse<object>(null, false, "Failed to delete claimant"));
            }
        }
    }
}
