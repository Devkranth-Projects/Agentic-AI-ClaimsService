using AutoMapper;
using FluentValidation;
using Health.Claims.Service.Application.DataTransferObjects;
using Health.Claims.Service.Application.Interfaces;
using Health.Claims.Service.Domain.Entities;
using Health.Claims.Service.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health.Claims.Service.Infrastructure.Services
{
    public class ClaimService : IClaimService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<SubmitClaimRequestDto> _validator;
        private readonly IMessageProducer _messageProducer;

        public ClaimService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<SubmitClaimRequestDto> validator,
            IMessageProducer messageProducer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
            _messageProducer = messageProducer;
        }

        public async Task<ClaimResponseDto> SubmitClaimAsync(SubmitClaimRequestDto requestDto)
        {
            // 1. Validation: Ensures the incoming data is clean
            var validationResult = await _validator.ValidateAsync(requestDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // 2. Mapping and Entity Creation
            var claimant = _mapper.Map<ClaimantEntity>(requestDto);
            var policy = _mapper.Map<PolicyEntity>(requestDto);
            var claim = _mapper.Map<ClaimRecordEntity>(requestDto);

            // Define default status (assuming 'Submitted' is one of your seeded statuses)
            // NOTE: Replace the Guid with the actual ID of your 'Submitted' status
            var defaultStatusId = Guid.Parse("01c0c6e0-2521-4f10-9c10-8b1e0c5f4d85");

            // 3. Establish Relationships (Foreign Keys)
            claim.ClaimantId = claimant.EntityId;
            claim.PolicyId = policy.EntityId;
            claim.StatusId = defaultStatusId;

            // 4. Persistence (using the Unit of Work)
            await _unitOfWork.ClaimantsRepo.AddNewClaimant(claimant);
            await _unitOfWork.PoliciesRepo.AddNewPolicy(policy); // Assuming IPolicyRepository has AddNewPolicyAsync
            await _unitOfWork.ClaimsRepo.SubmitNewClaimAsync(claim);     // Assuming IClaimRepository has AddNewClaimAsync

            // 5. Documents (if any)
            // You would typically handle documents here, mapping them from the DTO
            // and using your IDocumentRepository.

            // 6. Commit Transaction
            await _unitOfWork.CompleteAsync();

            // 7. Messaging (for asynchronous processing)
            var message = new ClaimSubmittedMessage
            {
                ClaimId = claim.EntityId,
                Description = claim.Description,
                Amount = claim.Amount
            };
            await _messageProducer.PublishMessage(message);

            // 8. Final Mapping and Return
            return _mapper.Map<ClaimResponseDto>(claim);
        }
    }
}