using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health.Claims.Service.Application.DataTransferObjects
{
    using System;

    public class ClaimAdjudicatedMessage
    {
        public Guid ClaimId { get; set; } 
        public string FinalStatus { get; set; } = string.Empty;
        public string AgentReasoning { get; set; } = string.Empty ;
        public decimal FinalAmount { get; set; } = decimal.Zero;
    }
}
