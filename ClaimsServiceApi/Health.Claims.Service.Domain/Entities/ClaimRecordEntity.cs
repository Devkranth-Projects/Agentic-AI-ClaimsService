using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;

namespace Health.Claims.Service.Domain.Entities
{
    public class ClaimRecordEntity : BaseEntity
    {
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime DateOfIncident { get; set; }
        public string IncidentLocation { get; set; } =string.Empty;


        // Foreign Keys for database relationships
        public Guid ClaimantId { get; set; }
        public Guid PolicyId { get; set; }
        public Guid StatusId { get; set; }

        // Navigation Properties for EF Core
        public virtual ClaimantEntity? Claimant { get; set; } 
        public virtual PolicyEntity? Policy { get; set; } 
        public virtual ClaimStatusEntity? Status { get; set; }
        public virtual ICollection<DocumentEntity> Documents { get; set; } = new List<DocumentEntity>();
    }
}
