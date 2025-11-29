using Health.Claims.Service.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Health.Claims.Service.Domain.Interfaces
{
    public interface IClaimantRepository
    {
        Task<IEnumerable<ClaimantEntity>> GetAllClaimants(bool includeDeleted = false);

        Task<ClaimantEntity?> GetDetailsByClaimantId(Guid claimAntId);
        Task AddNewClaimant(ClaimantEntity entity);
        Task EditExistingClaimant(ClaimantEntity entity);
        Task DeleteExistingClaimant(Guid claimAntId);

    }
}
