using Health.Claims.Service.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Health.Claims.Service.Domain.Interfaces
{
    public interface IClaimRepository
    {
        Task<ClaimRecordEntity?> GetDetailsByClaimIdAsync(Guid claimId);
        Task SubmitNewClaimAsync(ClaimRecordEntity entity);
    }
}
