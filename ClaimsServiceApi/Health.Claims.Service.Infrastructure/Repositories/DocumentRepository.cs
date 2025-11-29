using Health.Claims.Service.Domain.Entities;
using Health.Claims.Service.Domain.Interfaces;
using Health.Claims.Service.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Health.Claims.Service.Infrastructure.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly ClaimsDataServiceDBContext _context;

        public DocumentRepository(ClaimsDataServiceDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<DocumentEntity?> GetDocumentDetailsByClaimIdAsync(Guid claimId)
        {
            if (claimId == Guid.Empty)
                throw new ArgumentException("Claim ID cannot be empty.", nameof(claimId));

            return await _context.Documents
                .Include(d => d.Claim) // eager-load the claim entity if needed
                .FirstOrDefaultAsync(d => d.ClaimId == claimId && d.IsDeleted == false);
        }

        public async Task SubmitNewDocumentsForClaimIdAsync(DocumentEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.EntityId = Guid.NewGuid();
            entity.CreatedDt = DateTime.UtcNow;

            // TODO: Set CreatedBy from user context/security
            // entity.CreatedBy = currentUserId;

            await _context.Documents.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
    }
}
