using AutoMapper;
using RentFlex.Application.Models;
using RentFlex.Domain.entities;

namespace RentFlex.Application.MappingProfiles;
internal class EstateProfiles : Profile
{
    public EstateProfiles()
    {
        CreateMap<Estate, EstateDto>()
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Address.Country))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
            .ForMember(dest => dest.StreetName, opt => opt.MapFrom(src => src.Address.StreetName))
            .ForMember(dest => dest.PropertyNumber, opt => opt.MapFrom(src => src.Address.PropertyNumber));
    }
}
