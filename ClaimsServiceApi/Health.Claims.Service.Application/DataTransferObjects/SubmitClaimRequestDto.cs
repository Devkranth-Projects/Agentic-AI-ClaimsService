using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health.Claims.Service.Application.DataTransferObjects
{
    using System;
    using System.Collections.Generic;

    public class SubmitClaimRequestDto
    {
        // Claim properties
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime DateOfIncident { get; set; }
        public string IncidentLocation { get; set; } = string.Empty;

        // Claimant properties (nested object)
        public ClaimantDto Claimant { get; set; } = new ClaimantDto();

        // Policy properties
        public string PolicyNumber { get; set; } = string.Empty;


        // List of documents (nested DTOs)
        public List<DocumentUploadDto>? Documents { get; set; }
    }
}
