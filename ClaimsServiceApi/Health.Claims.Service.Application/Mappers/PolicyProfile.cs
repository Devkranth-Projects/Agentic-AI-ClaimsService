using AutoMapper;
using Health.Claims.Service.Application.DataTransferObjects;
using Health.Claims.Service.Domain.Entities;

namespace Health.Claims.Service.Application.Mappers
{
    public class PolicyProfile : Profile
    {
        public PolicyProfile()
        {
            // Map PolicyDto -> PolicyEntity
            CreateMap<PolicyDto, PolicyEntity>()
                .ForMember(dest => dest.EntityId, opt => opt.MapFrom(src => src.PolicyId))
                .ForMember(dest => dest.PolicyNumber, opt => opt.MapFrom(src => src.PolicyNumber))
                .ForMember(dest => dest.PolicyType, opt => opt.MapFrom(src => src.PolicyType))
                .ForMember(dest => dest.EffectiveDate, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.StartDate, DateTimeKind.Utc)))
                .ForMember(dest => dest.ExpirationDate, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.EndDate, DateTimeKind.Utc)))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.ClaimantId, opt => opt.MapFrom(src => src.ClaimantId))
                // Prevent nested entity mapping loop
                .ForMember(dest => dest.Claimant, opt => opt.Ignore());

            // Map PolicyEntity -> PolicyDto (with Claimant details)
            CreateMap<PolicyEntity, PolicyDto>()
                .ForMember(dest => dest.PolicyId, opt => opt.MapFrom(src => src.EntityId))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.EffectiveDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.ExpirationDate))
                .ForMember(dest => dest.PolicyNumber, opt => opt.MapFrom(src => src.PolicyNumber))
                .ForMember(dest => dest.PolicyType, opt => opt.MapFrom(src => src.PolicyType))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.ClaimantId, opt => opt.MapFrom(src => src.ClaimantId))
                // ✅ This maps the full ClaimantDto when Claimant entity is included
                .ForMember(dest => dest.ClaimantDetails, opt => opt.MapFrom(src => src.Claimant));
        }
    }
}
