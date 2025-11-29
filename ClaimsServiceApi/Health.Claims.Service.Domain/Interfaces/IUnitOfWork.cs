using System;
using System.Threading.Tasks;

namespace Health.Claims.Service.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IClaimantRepository ClaimantsRepo { get; }
        IClaimRepository ClaimsRepo { get; }
        IPolicyRepository PoliciesRepo { get; }
        IClaimStatusRepository ClaimStatusesRepo { get; }
        IDocumentRepository DocumentsRepo { get; }

        Task<int> CompleteAsync();
    }
}
