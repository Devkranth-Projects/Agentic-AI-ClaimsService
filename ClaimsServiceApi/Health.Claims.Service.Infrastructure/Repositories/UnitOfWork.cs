using Health.Claims.Service.Domain.Interfaces;
using Health.Claims.Service.Infrastructure.Data.Context;
using Health.Claims.Service.Infrastructure.Repositories;
using System;
using System.Threading.Tasks;

namespace Health.Claims.Service.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ClaimsDataServiceDBContext _context;

        public IClaimantRepository ClaimantsRepo { get; }
        public IClaimRepository ClaimsRepo { get; }
        public IPolicyRepository PoliciesRepo { get; }
        public IClaimStatusRepository ClaimStatusesRepo { get; }
        public IDocumentRepository DocumentsRepo { get; }

        public UnitOfWork(ClaimsDataServiceDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

            ClaimantsRepo = new ClaimantRepository(_context);
            ClaimsRepo = new ClaimRepository(_context);
            PoliciesRepo = new PolicyRepository(_context);
            ClaimStatusesRepo = new ClaimStatusRepository(_context);
            DocumentsRepo = new DocumentRepository(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
