using Health.Claims.Service.Domain.Entities;

namespace Health.Claims.Service.Domain.Interfaces
{
    public interface IDocumentRepository
    {
        Task<DocumentEntity?> GetDocumentDetailsByClaimIdAsync(Guid claimId);
        Task SubmitNewDocumentsForClaimIdAsync(DocumentEntity entity);
    }
}
