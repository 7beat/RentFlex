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
            .ForMember(dest => dest.PropertyNumber, opt => opt.MapFrom(src => src.Address.PropertyNumber))
            .ForMember(dest => dest.PublishedAirbnb, opt => opt.MapFrom(src => src.AirbnbReference != null));

        CreateMap<UpsertEstateCommand, Estate>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
            {
                Country = src.Country!,
                City = src.City!,
                PostalCode = src.PostalCode!,
                StreetName = src.StreetName!,
                PropertyNumber = src.PropertyNumber.HasValue ? (int)src.PropertyNumber : default
            }))
            .ForMember(dest => dest.ImageUrls, opt => opt.Ignore())
            .ReverseMap();


        CreateMap<EstateDto, UpsertEstateCommand>()
            .ForMember(dest => dest.PublishAirbnb, opt => opt.MapFrom(src => src.PublishedAirbnb))
            .ReverseMap();
    }
}
