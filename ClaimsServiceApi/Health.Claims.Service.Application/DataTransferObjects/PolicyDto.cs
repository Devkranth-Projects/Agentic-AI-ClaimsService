using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health.Claims.Service.Application.DataTransferObjects
{
    public class PolicyDto
    {
        public Guid PolicyId { get; set; }
        public Guid ClaimantId { get; set; }
        public string PolicyNumber { get; set; } = string.Empty;
        public string PolicyType { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Description { get; set; }
        public ClaimantDto? ClaimantDetails { get; set; } 
    }

}
