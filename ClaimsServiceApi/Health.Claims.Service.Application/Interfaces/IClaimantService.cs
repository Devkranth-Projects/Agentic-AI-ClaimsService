using Health.Claims.Service.Application.DataTransferObjects;
using Health.Claims.Service.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health.Claims.Service.Application.Interfaces
{
    /// <summary>
    /// Defines service-level operations for managing claimants.
    /// </summary>
    public interface IClaimantService
    {
        /// <summary>
        /// Retrieves all claimants (optionally active only).
        /// </summary>
        Task<IEnumerable<ClaimantDto>> GetAllClaimantsAsync(bool includeDeleted = false);

        /// <summary>
        /// Retrieves a claimant by unique identifier.
        /// </summary>
        Task<ClaimantDto?> GetClaimantByIdAsync(Guid claimantId);

        /// <summary>
        /// Adds a new claimant.
        /// </summary>
        Task AddClaimantAsync(ClaimantDto entity);

        /// <summary>
        /// Updates an existing claimant.
        /// </summary>
        Task UpdateClaimantAsync(ClaimantDto entity);

        /// <summary>
        /// Deletes (soft deletes) a claimant.
        /// </summary>
        Task DeleteClaimantAsync(Guid claimantId);
    }
}
