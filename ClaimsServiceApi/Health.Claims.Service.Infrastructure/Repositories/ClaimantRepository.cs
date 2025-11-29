using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Health.Claims.Service.Domain.Entities;
using Health.Claims.Service.Domain.Interfaces;
using Health.Claims.Service.Infrastructure.Data.Context;

namespace Health.Claims.Service.Infrastructure.Repositories
{
    /// <summary>
    /// Repository responsible for performing CRUD operations on Claimant entities.
    /// </summary>
    public class ClaimantRepository : IClaimantRepository
    {
        private readonly ClaimsDataServiceDBContext _dbContext;

        public ClaimantRepository(ClaimsDataServiceDBContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <summary>
        /// Retrieves a claimant with related claims and policies by claimant ID.
        /// </summary>
        public async Task<ClaimantEntity?> GetDetailsByClaimantId(Guid claimantId)
        {
            if (claimantId == Guid.Empty)
                throw new ArgumentException("Claimant ID cannot be empty.", nameof(claimantId));

            return await _dbContext.Claimants
                                   .AsNoTracking()
                                   .Include(c => c.Claims)
                                   .Include(c => c.Policies)
                                   .FirstOrDefaultAsync(c => c.EntityId == claimantId && c.IsDeleted == false);
        }

        /// <summary>
        /// Adds a new claimant to the database.
        /// </summary>
        public async Task AddNewClaimant(ClaimantEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.EntityId = Guid.NewGuid();
            entity.CreatedDt = DateTime.UtcNow;
            entity.IsDeleted = false;

            await _dbContext.Claimants.AddAsync(entity);
            await SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing claimant’s information.
        /// </summary>
        public async Task EditExistingClaimant(ClaimantEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var existingClaimant = await _dbContext.Claimants
                .FirstOrDefaultAsync(c => c.EntityId == entity.EntityId && c.IsDeleted == false);

            if (existingClaimant == null)
                throw new InvalidOperationException($"Claimant with ID '{entity.EntityId}' not found.");

            // --- Personal Info ---
            existingClaimant.FirstName = entity.FirstName;
            existingClaimant.MiddleName = entity.MiddleName;
            existingClaimant.LastName = entity.LastName;
            existingClaimant.Dob = entity.Dob;
            existingClaimant.MaritalStatus = entity.MaritalStatus;
            existingClaimant.Nationality = entity.Nationality;

            // --- Contact Info ---
            existingClaimant.Email = entity.Email;
            existingClaimant.ConfirmEmail = entity.ConfirmEmail;
            existingClaimant.Phone = entity.Phone;
            existingClaimant.AltPhone = entity.AltPhone;

            // --- Address ---
            existingClaimant.AddressLine1 = entity.AddressLine1;
            existingClaimant.AddressLine2 = entity.AddressLine2;
            existingClaimant.City = entity.City;
            existingClaimant.State = entity.State;
            existingClaimant.Zip = entity.Zip;
            existingClaimant.Country = entity.Country;

            // --- Identification ---
            existingClaimant.Passport = entity.Passport;
            existingClaimant.DriverLicense = entity.DriverLicense;
            existingClaimant.TaxId = entity.TaxId;

            // --- Credit Card Info ---
            existingClaimant.CardNumber = entity.CardNumber;
            existingClaimant.CardExpiry = entity.CardExpiry;
            existingClaimant.CardCVV = entity.CardCVV;
            existingClaimant.CardHolder = entity.CardHolder;

            // --- Additional Info ---
            existingClaimant.Notes = entity.Notes;

            // --- Audit ---
            existingClaimant.UpdatedDt = DateTime.UtcNow;

            _dbContext.Claimants.Update(existingClaimant);
            await SaveChangesAsync();
        }

        /// <summary>
        /// Performs a soft delete of a claimant by setting IsDeleted flag.
        /// </summary>
        public async Task DeleteExistingClaimant(Guid claimantId)
        {
            if (claimantId == Guid.Empty)
                throw new ArgumentException("Claimant ID cannot be empty.", nameof(claimantId));

            var claimant = await _dbContext.Claimants
                .FirstOrDefaultAsync(c => c.EntityId == claimantId && c.IsDeleted == false);

            if (claimant == null)
                throw new InvalidOperationException($"Claimant with ID '{claimantId}' not found.");

            claimant.IsDeleted = true;
            claimant.UpdatedDt = DateTime.UtcNow;

            _dbContext.Claimants.Update(claimant);
            await SaveChangesAsync();
        }

        /// <summary>
        /// Saves all pending changes to the database.
        /// </summary>
        private async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ClaimantEntity>> GetAllClaimants(bool includeDeleted = false)
        {
            var query = _dbContext.Claimants
                                  .AsNoTracking()
                                  .Include(c => c.Claims)
                                  .Include(c => c.Policies)
                                  .AsQueryable();

            if (!includeDeleted)
                query = query.Where(c => c.IsDeleted == false);

            return await query.ToListAsync();
        }

    }
}
