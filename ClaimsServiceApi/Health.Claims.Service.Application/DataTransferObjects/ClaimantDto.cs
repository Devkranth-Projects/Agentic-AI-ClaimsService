using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health.Claims.Service.Application.DataTransferObjects
{
    public class ClaimantDto
    {
        public Guid ClaimantId { get; set; }
        // Personal Information
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public DateTime Dob { get; set; }
        public string? MaritalStatus { get; set; }
        public string? Nationality { get; set; }

        // Contact Information
        public string Email { get; set; } = string.Empty;
        public string ConfirmEmail { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? AltPhone { get; set; }

        // Address
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public string? Country { get; set; }

        // Identification
        public string? Passport { get; set; }
        public string? DriverLicense { get; set; }
        public string? TaxId { get; set; }

        // Credit Card Info
        public string? CardNumber { get; set; }
        public string? CardExpiry { get; set; }
        public string? CardCVV { get; set; }
        public string? CardHolder { get; set; }

        // Additional
        public string? Notes { get; set; }
    }
}
