using AutoMapper;
using Health.Claims.Service.Application.DataTransferObjects;
using Health.Claims.Service.Application.Interfaces;
using Health.Claims.Service.Domain.Entities;
using Health.Claims.Service.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Health.Claims.Service.Infrastructure.Services
{
    /// <summary>
    /// Provides business logic and validation for managing Claimants.
    /// </summary>
    public class ClaimantService : IClaimantService
    {
        private readonly IClaimantRepository _claimantRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ClaimantService> _logger;

        public ClaimantService(
            IClaimantRepository claimantRepository,
            IMapper mapper,
            ILogger<ClaimantService> logger)
        {
            _claimantRepository = claimantRepository ?? throw new ArgumentNullException(nameof(claimantRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<ClaimantDto>> GetAllClaimantsAsync(bool includeDeleted = false)
        {
            try
            {
                var entities = await _claimantRepository.GetAllClaimants(includeDeleted);
                return _mapper.Map<IEnumerable<ClaimantDto>>(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching claimants");
                throw;
            }
        }

        public async Task<ClaimantDto?> GetClaimantByIdAsync(Guid claimantId)
        {
            if (claimantId == Guid.Empty)
                throw new ArgumentException("Invalid claimant ID.", nameof(claimantId));

            var entity = await _claimantRepository.GetDetailsByClaimantId(claimantId);
            return entity == null ? null : _mapper.Map<ClaimantDto>(entity);
        }

        public async Task AddClaimantAsync(ClaimantDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            _logger.LogInformation("Adding new claimant: {FirstName} {LastName}", dto.FirstName, dto.LastName);
            var entity = _mapper.Map<ClaimantEntity>(dto);
            await _claimantRepository.AddNewClaimant(entity);
        }

        public async Task UpdateClaimantAsync(ClaimantDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            _logger.LogInformation("Updating claimant with ID: {ClaimantId}", dto.ClaimantId);
            var entity = _mapper.Map<ClaimantEntity>(dto);
            await _claimantRepository.EditExistingClaimant(entity);
        }

        public async Task DeleteClaimantAsync(Guid claimantId)
        {
            if (claimantId == Guid.Empty)
                throw new ArgumentException("Invalid claimant ID.", nameof(claimantId));

            _logger.LogInformation("Deleting claimant with ID: {ClaimantId}", claimantId);
            await _claimantRepository.DeleteExistingClaimant(claimantId);
        }
    }
}
