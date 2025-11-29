using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health.Claims.Service.Application.DataTransferObjects
{
    using System;

    public class ClaimSubmittedMessage
    {
        public Guid ClaimId { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; } = decimal.Zero;
        public string PolicyNumber { get; set; } = string.Empty;
    }
}
