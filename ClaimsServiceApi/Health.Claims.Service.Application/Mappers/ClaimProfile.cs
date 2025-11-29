using AutoMapper;
using Health.Claims.Service.Application.DataTransferObjects;
using Health.Claims.Service.Domain.Entities;

namespace Health.Claims.Service.Application.Mappers
{
    public class ClaimProfile : Profile
    {
        public ClaimProfile()
        {
            // 1. DTO → Domain Entity Mapping
            CreateMap<SubmitClaimRequestDto, ClaimRecordEntity>()
                .ForMember(dest => dest.EntityId, opt => opt.Ignore())
                .ForMember(dest => dest.Claimant, opt => opt.MapFrom(src => src.Claimant)) // Map nested ClaimantDto
                .ForMember(dest => dest.Policy, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore());

            // 2. Domain Entity → DTO Mapping (outgoing)
            CreateMap<ClaimRecordEntity, ClaimResponseDto>()
               .ForMember(dest => dest.ClaimantName, opt => opt.MapFrom(src => src.Claimant!.FirstName + " " + src.Claimant!.LastName))
               .ForMember(dest => dest.ClaimantEmail, opt => opt.MapFrom(src => src.Claimant!.Email))
               .ForMember(dest => dest.PolicyNumber, opt => opt.MapFrom(src => src.Policy!.PolicyNumber))
               .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status!.StatusName));
        }
    }
}
