using AutoMapper;
using RentFlex.Application.Models;
using RentFlex.Domain.entities;

namespace RentFlex.Application.MappingProfiles;
internal class RentalProfiles : Profile
{
    public RentalProfiles()
    {
        CreateMap<Rental, RentalDto>()
            .ForMember(dest => dest.PropertyName, opt => opt.MapFrom(src => src.Estate.PropertyName))
            .ReverseMap();
    }
}
