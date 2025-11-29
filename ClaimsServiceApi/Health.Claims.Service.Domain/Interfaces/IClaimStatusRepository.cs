using Health.Claims.Service.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health.Claims.Service.Domain.Interfaces
{
    public  interface IClaimStatusRepository
    {
        Task<ClaimStatusEntity?> GetClaimStatusDetailsByClaimIdAsync(Guid claimId);
        Task AddNewClaimStatusAsync(ClaimStatusEntity entity);
    }
}
