using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health.Claims.Service.Application.DataTransferObjects
{
    using System;

    public class ClaimResponseDto
    {
        public Guid ClaimId { get; set; }
        public string Description { get; set; }= string.Empty;
        public decimal Amount { get; set; }
        public DateTime DateOfIncident { get; set; }
        public string Status { get; set; }= string.Empty ;

        public string ClaimantName { get; set; } = string.Empty ;
        public string ClaimantEmail { get; set; } =string.Empty ;

        public string PolicyNumber { get; set; } = string.Empty ;
    }
}
