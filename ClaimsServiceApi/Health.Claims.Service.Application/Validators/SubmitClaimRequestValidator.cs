using FluentValidation;
using Health.Claims.Service.Application.DataTransferObjects;

namespace Health.Claims.Service.Application.Validators
{
    public class SubmitClaimRequestValidator : AbstractValidator<SubmitClaimRequestDto>
    {
        public SubmitClaimRequestValidator()
        {
            // Claim properties
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MinimumLength(10).WithMessage("Description must be at least 10 characters long.");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");

            RuleFor(x => x.DateOfIncident)
                .NotEmpty().WithMessage("Date of incident is required.")
                .LessThanOrEqualTo(DateTime.Today).WithMessage("Date of incident cannot be in the future.");

            RuleFor(x => x.IncidentLocation)
                .NotEmpty().WithMessage("Incident location is required.");

            // Policy properties
            RuleFor(x => x.PolicyNumber)
                .NotEmpty().WithMessage("Policy number is required.");

            // Nested Claimant validation
            RuleFor(x => x.Claimant).NotNull().WithMessage("Claimant information is required.");

            RuleFor(x => x.Claimant!.FirstName)
                .NotEmpty().WithMessage("Claimant first name is required.");

            RuleFor(x => x.Claimant!.LastName)
                .NotEmpty().WithMessage("Claimant last name is required.");

            RuleFor(x => x.Claimant!.Email)
                .NotEmpty().WithMessage("Claimant email is required.")
                .EmailAddress().WithMessage("A valid email address is required.");

            RuleFor(x => x.Claimant!.ConfirmEmail)
                .Equal(x => x.Claimant!.Email).WithMessage("Confirm email must match email.");

            RuleFor(x => x.Claimant!.Phone)
                .NotEmpty().WithMessage("Claimant phone number is required.");
        }
    }
}
