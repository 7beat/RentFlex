using AutoMapper;
using RentFlex.Application.Features.Estates.Commands;
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
            .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.Address.PostalCode))
            .ForMember(dest => dest.StreetName, opt => opt.MapFrom(src => src.Address.StreetName))
            .ForMember(dest => dest.PropertyNumber, opt => opt.MapFrom(src => src.Address.PropertyNumber));

        CreateMap<UpsertEstateCommand, Estate>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
            {
                Country = src.Country!,
                City = src.City!,
                PostalCode = src.PostalCode!,
                StreetName = src.StreetName!,
                PropertyNumber = src.PropertyNumber.HasValue ? (int)src.PropertyNumber : default
            }))
            .ReverseMap();


        CreateMap<UpsertEstateCommand, EstateDto>()
            .ReverseMap();
    }
}
