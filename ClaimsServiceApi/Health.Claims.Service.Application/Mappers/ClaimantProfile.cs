using AutoMapper;
using Health.Claims.Service.Application.DataTransferObjects;
using Health.Claims.Service.Domain.Entities;

namespace Health.Claims.Service.Application.Mappers
{
    public class ClaimantProfile : Profile
    {
        public ClaimantProfile()
        {
            // Map both ways between DTO and Entity
            CreateMap<ClaimantEntity, ClaimantDto>()
                .ForMember(dest => dest.ClaimantId, opt => opt.MapFrom(src => src.EntityId))
                .ReverseMap()
                .ForMember(dest => dest.EntityId, opt => opt.MapFrom(src => src.ClaimantId));
        }

    }
}
