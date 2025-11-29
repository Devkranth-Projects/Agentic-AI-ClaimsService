using System;
using System.Collections.Generic;

namespace Health.Claims.Service.Domain.Entities
{
    public class PolicyEntity : BaseEntity
    {
        // Policy details
        public string PolicyNumber { get; set; } = string.Empty;
        public string PolicyType { get; set; } = string.Empty; // Added policy type
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string? Description { get; set; } // Optional description

        // Relationship to Claimant
        public Guid ClaimantId { get; set; }
        public virtual ClaimantEntity? Claimant { get; set; }

        // Claims under this policy
        public virtual ICollection<ClaimRecordEntity> Claims { get; set; } = new List<ClaimRecordEntity>();

        
    }
}
