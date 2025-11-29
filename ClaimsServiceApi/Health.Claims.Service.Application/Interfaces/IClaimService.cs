using Health.Claims.Service.Application.DataTransferObjects;

namespace Health.Claims.Service.Application.Interfaces
{
    // Returns a DTO instead of the domain entity
    public interface IClaimService
    {
        Task<ClaimResponseDto> SubmitClaimAsync(SubmitClaimRequestDto requestDto);
    }
}
